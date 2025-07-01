using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Integration.BrokerMessageSender;
using AxiteHR.Integration.BrokerMessageSender.Models;
using AxiteHR.Integration.GlobalClass.Enums;
using AxiteHR.Integration.GlobalClass.Enums.Invoice;
using AxiteHR.Services.InvoiceAPI.Data;
using AxiteHR.Services.InvoiceAPI.Helpers;
using AxiteHR.Services.InvoiceAPI.Models;
using AxiteHR.Services.InvoiceAPI.Models.Dto.Generator;
using AxiteHR.Services.InvoiceAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Serilog;
using System.Data;

namespace AxiteHR.Services.InvoiceAPI.Services.Generator.Impl
{
	public class InvoiceGenerator(
		AppDbContext dbContext,
		IConfiguration configuration,
		IServiceProvider serviceProvider,
		IStringLocalizer<InvoiceResources> invoiceLocalizer,
		IOptions<RabbitMqMessageSenderConfig> rabbitMqMessageSenderConfig) : IInvoiceGenerator
	{
		private readonly RabbitMqMessageSenderConfig _rabbitMqMessageSenderConfig = rabbitMqMessageSenderConfig.Value;

		public async Task<InvoiceGeneratorResponseDto> GenerateInvoiceAsync(InvoiceGeneratorRequestDto requestDto)
		{
			try
			{
				//New invoice is always Invoice type
				const InvoiceType invoiceType = InvoiceType.Invoice;

				var number = await ReserveNumberAsync(invoiceType, requestDto.CompanyUserId, requestDto.IssueDate.Year, requestDto.IssueDate.Month);
				var invoiceNumber = InvoiceNumberCreator.GetInvoiceNumber(invoiceType, requestDto.IssueDate.Year, requestDto.IssueDate.Month, number);

				await using var transaction = await dbContext.Database.BeginTransactionAsync();

				var invoice = MapInvoiceFromRequest(ref requestDto, invoiceType, invoiceNumber);

				dbContext.Invoices.Add(invoice);

				await dbContext.SaveChangesAsync();
				await transaction.CommitAsync();

				await PublishGenerateInvoiceAsync(requestDto, Language.pl);
				await PublishGenerateInvoiceAsync(requestDto, Language.en);

				return new InvoiceGeneratorResponseDto
				{
					IsSucceeded = true
				};
			}
			catch (Exception ex)
			{
				var param = new
				{
					requestDto.InsUserId,
					requestDto.CompanyId,
					requestDto.CompanyUserId,
					DateOfIssue = DateTime.UtcNow
				};
				Log.Error(ex, "Error while generating invoice. Param: {Param}", param);

				return new InvoiceGeneratorResponseDto
				{
					IsSucceeded = false,
					ErrorMessage = invoiceLocalizer[InvoiceResourcesKeys.Invoice_Generator_Error]
				};
			}
		}

		private async Task<int> ReserveNumberAsync(InvoiceType type, int companyUserId, int year, int month)
		{
			await using var transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);

			var sequence = await dbContext.InvoiceSequences
				.FromSqlRaw(@"
					SELECT * FROM InvoiceSequences WITH (UPDLOCK, ROWLOCK)
					WHERE Type = {0} AND CompanyUserId = {1} AND Year = {2} AND Month = {3}",
					type, companyUserId, year, month)
				.SingleOrDefaultAsync();

			if (sequence == null)
			{
				sequence = new InvoiceSequence
				{
					Type = type,
					CompanyUserId = companyUserId,
					Year = year,
					Month = month,
					CurrentNumber = 1
				};

				dbContext.InvoiceSequences.Add(sequence);
			}
			else
			{
				sequence.CurrentNumber++;
			}

			await dbContext.SaveChangesAsync();
			await transaction.CommitAsync();

			return sequence.CurrentNumber;
		}

		private static Invoice MapInvoiceFromRequest(ref InvoiceGeneratorRequestDto requestDto, InvoiceType invoiceType, string invoiceNumber)
		{
			var invoice = new Invoice
			{
				Status = InvoiceStatus.Unpaid,
				Type = invoiceType,
				Number = invoiceNumber,
				CompanyId = requestDto.CompanyId,
				CompanyUserId = requestDto.CompanyUserId,
				BlobFileName = Guid.NewGuid().ToString() + ".pdf",
				ClientName = requestDto.ClientName,
				ClientNip = requestDto.ClientNip,
				ClientStreet = requestDto.ClientStreet,
				ClientHouseNumber = requestDto.ClientHouseNumber,
				ClientPostalCode = requestDto.ClientPostalCode,
				ClientCity = requestDto.ClientCity,
				RecipientName = requestDto.RecipientName,
				RecipientNip = requestDto.RecipientNip,
				RecipientStreet = requestDto.RecipientStreet,
				RecipientHouseNumber = requestDto.RecipientHouseNumber,
				RecipientPostalCode = requestDto.RecipientPostalCode,
				RecipientCity = requestDto.RecipientCity,
				IssueDate = requestDto.IssueDate,
				SaleDate = requestDto.SaleDate,
				PaymentMethod = requestDto.PaymentMethod,
				BankAccountNumber = requestDto.BankAccountNumber,
				PaymentDeadline = requestDto.PaymentDeadline,
				Currency = requestDto.Currency,
				IsSplitPayment = requestDto.IsSplitPayment,

				InsUserId = requestDto.InsUserId,
				InsDate = DateTime.UtcNow,
				UpdUserId = requestDto.InsUserId,
				UpdDate = DateTime.UtcNow,
			};

			decimal netAmonut = 0;
			decimal grossAmount = 0;
			decimal vatAmount = 0;

			foreach (var positionRequestDto in requestDto.InvoicePositions)
			{
				var invoicePosition = new InvoicePosition
				{
					Invoice = invoice,
					ProductName = positionRequestDto.ProductName,
					Unit = positionRequestDto.Unit,
					Quantity = positionRequestDto.Quantity,
					NetPrice = positionRequestDto.NetPrice,
					VatRate = positionRequestDto.VatRate
				};

				invoicePosition.NetAmount = Math.Round(invoicePosition.NetPrice * invoicePosition.Quantity, 2, MidpointRounding.AwayFromZero);
				invoicePosition.VatAmount = Math.Round(invoicePosition.NetAmount * (invoicePosition.VatRate / 100m), 2, MidpointRounding.AwayFromZero);
				invoicePosition.GrossAmount = Math.Round(invoicePosition.NetAmount + invoicePosition.VatAmount, 2, MidpointRounding.AwayFromZero);

				positionRequestDto.NetAmount = invoicePosition.NetAmount;
				positionRequestDto.VatAmount = invoicePosition.VatAmount;
				positionRequestDto.GrossAmount = invoicePosition.GrossAmount;

				netAmonut += invoicePosition.NetAmount;
				grossAmount += invoicePosition.GrossAmount;
				vatAmount += invoicePosition.VatAmount;

				invoice.InvoicePositions.Add(invoicePosition);
			}

			invoice.NetAmount = Math.Round(netAmonut, 2, MidpointRounding.AwayFromZero);
			invoice.GrossAmount = Math.Round(grossAmount, 2, MidpointRounding.AwayFromZero);

			requestDto.NetAmount = invoice.NetAmount;
			requestDto.GrossAmount = invoice.GrossAmount;
			requestDto.VatAmount = Math.Round(vatAmount, 2, MidpointRounding.AwayFromZero);
			requestDto.BlobFileName = invoice.BlobFileName;
			requestDto.InvoiceNumber = invoiceNumber;

			return invoice;
		}

		public async Task PublishGenerateInvoiceAsync(InvoiceGeneratorRequestDto requestDto, Language language)
		{
			requestDto.Language = language;
			var fileName = requestDto.BlobFileName;
			var fileNameSplit = fileName.Split('.');

			requestDto.BlobFileName = $"{fileNameSplit[0]}_{language.ToString().ToUpper()}.{fileNameSplit[1]}";

			MessageSenderModel<RabbitMqMessageSenderConfig, InvoiceGeneratorRequestDto> messageSenderModel = new()
			{
				Message = requestDto,
				Config = _rabbitMqMessageSenderConfig
			};
			messageSenderModel.Config.QueueName = configuration.GetValue<string>(ConfigurationHelper.InvoiceGeneratorQueue)!;

			var messagePublisher = serviceProvider.GetService<MessagePublisher>() ?? throw new NotSupportedException("No such service for MessagePublisher");
			await messagePublisher.PublishMessageAsync(messageSenderModel);

			requestDto.BlobFileName = fileName;
		}
	}
}
