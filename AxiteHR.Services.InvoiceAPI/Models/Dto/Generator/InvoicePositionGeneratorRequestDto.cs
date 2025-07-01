using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Integration.GlobalClass.Enums.Invoice;
using AxiteHR.Services.InvoiceAPI.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AxiteHR.Services.InvoiceAPI.Models.Dto.Generator
{
	public record InvoicePositionGeneratorRequestDto
	{
		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[MaxLength(100, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_MaxLengthField)]
		public string ProductName { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		public Unit Unit { get; set; }

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[DecimalPrecision(2, ErrorMessageResourceType = typeof(InvoiceResources), ErrorMessageResourceName = InvoiceResourcesKeys.Invoice_InvalidDecimalPrecision)]
		public decimal Quantity { get; set; }

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[DecimalPrecision(2, ErrorMessageResourceType = typeof(InvoiceResources), ErrorMessageResourceName = InvoiceResourcesKeys.Invoice_InvalidDecimalPrecision)]
		public decimal NetPrice { get; set; }

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		public int VatRate { get; set; }

		/// <summary>
		/// Not send by request, completed in the service for message bus
		/// </summary>
		public decimal? VatAmount { get; set; }

		/// <summary>
		/// Not send by request, completed in the service for message bus
		/// </summary>
		public decimal? NetAmount { get; set; }

		/// <summary>
		/// Not send by request, completed in the service for message bus
		/// </summary>
		public decimal? GrossAmount { get; set; }
	}
}
