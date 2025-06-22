using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.GlobalizationResources;
using System.ComponentModel.DataAnnotations;
using AxiteHR.Services.InvoiceAPI.Attributes;
using AxiteHR.Services.InvoiceAPI.Models.Enums;

namespace AxiteHR.Services.InvoiceAPI.Models.Dto.Generator
{
	public record InvoiceGeneratorRequestDto
	{
		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		public int CompanyId { get; set; }

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		public int CompanyUserId { get; set; }

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[MaxLength(100, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_MaxLengthField)]
		public string ClientName { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[Nip(ErrorMessageResourceType = typeof(InvoiceResources), ErrorMessageResourceName = InvoiceResourcesKeys.Invoice_InvalidNip)]
		public string ClientNip { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[MaxLength(100, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_MaxLengthField)]
		public string ClientStreet { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[MaxLength(30, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_MaxLengthField)]
		public string ClientHouseNumber { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[PostalCode(ErrorMessageResourceType = typeof(InvoiceResources), ErrorMessageResourceName = InvoiceResourcesKeys.Invoice_InvalidPostalCode)]
		public string ClientPostalCode { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[MaxLength(100, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_MaxLengthField)]
		public string ClientCity { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		public DateTime IssueDate { get; set; }

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		public DateTime SaleDate { get; set; }

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		public PaymentMethod PaymentMethod { get; set; }

		[RequiredIf(nameof(PaymentMethod), PaymentMethod.Transfer, ErrorMessageResourceType = typeof(InvoiceResources), ErrorMessageResourceName = InvoiceResourcesKeys.Invoice_BankNumberRequiredPaymentMethod)]
		public string BankAccountNumber { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		public DateTime PaymentDeadline { get; set; }

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		public Currency Currency { get; set; }

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		public bool IsSplitPayment { get; set; }

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		public Guid InsUserId { get; set; }

		/// <summary>
		/// Not send by request, completed in the service for message bus
		/// </summary>
		public decimal? NetAmount { get; set; }

		/// <summary>
		/// Not send by request, completed in the service for message bus
		/// </summary>
		public decimal? GrossAmount { get; set; }

		/// <summary>
		/// Not send by request, completed in the service for message bus
		/// </summary>
		public string BlobFileName { get; set; } = string.Empty;

		public IList<InvoicePositionGeneratorRequestDto> InvoicePositions { get; set; } = [];
	}
}
