using AxiteHR.Integration.BrokerMessageSender;
using AxiteHR.Integration.BrokerMessageSender.Models;
using AxiteHR.Services.InvoiceAPI.Data;
using AxiteHR.Services.InvoiceAPI.Helpers;
using AxiteHR.Services.InvoiceAPI.Models;
using AxiteHR.Services.InvoiceAPI.Models.Dto.Generator;
using AxiteHR.Services.InvoiceAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;

namespace AxiteHR.Services.InvoiceAPI.Services.Generator.Impl
{
	public class InvoiceGenerator(
		AppDbContext dbContext,
		IConfiguration configuration,
		IServiceProvider serviceProvider,
		IOptions<RabbitMqMessageSenderConfig> rabbitMqMessageSenderConfig) : IInvoiceGenerator
	{
		private readonly RabbitMqMessageSenderConfig _rabbitMqMessageSenderConfig = rabbitMqMessageSenderConfig.Value;

		public async Task<InvoiceGeneratorResponseDto> GenerateInvoiceAsync(InvoiceGeneratorRequestDto requestDto)
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

			await PublishGenerateInvoiceAsync(requestDto);

			return new InvoiceGeneratorResponseDto
			{
				IsSucceeded = true
			};
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

				invoicePosition.NetAmount = invoicePosition.NetPrice * invoicePosition.Quantity;
				invoicePosition.VatAmount = invoicePosition.NetAmount * (invoicePosition.VatRate / 100m);
				invoicePosition.GrossAmount = invoicePosition.NetAmount + invoicePosition.VatAmount;

				positionRequestDto.NetAmount = invoicePosition.NetAmount;
				positionRequestDto.VatAmount = invoicePosition.VatAmount;
				positionRequestDto.GrossAmount = invoicePosition.GrossAmount;

				netAmonut += invoicePosition.NetAmount;
				grossAmount += invoicePosition.GrossAmount;

				invoice.InvoicePositions.Add(invoicePosition);
			}

			invoice.NetAmount = netAmonut;
			invoice.GrossAmount = grossAmount;

			requestDto.NetAmount = invoice.NetAmount;
			requestDto.GrossAmount = invoice.GrossAmount;
			requestDto.BlobFileName = invoice.BlobFileName;

			return invoice;
		}

		public async Task PublishGenerateInvoiceAsync(InvoiceGeneratorRequestDto requestDto)
		{
			MessageSenderModel<RabbitMqMessageSenderConfig, InvoiceGeneratorRequestDto> messageSenderModel = new()
			{
				Message = requestDto,
				Config = _rabbitMqMessageSenderConfig
			};
			messageSenderModel.Config.QueueName = configuration.GetValue<string>(ConfigurationHelper.InvoiceGeneratorQueue)!;

			var messagePublisher = serviceProvider.GetService<MessagePublisher>() ?? throw new NotSupportedException("No such service for MessagePublisher");
			await messagePublisher.PublishMessageAsync(messageSenderModel);
		}
	}
}
