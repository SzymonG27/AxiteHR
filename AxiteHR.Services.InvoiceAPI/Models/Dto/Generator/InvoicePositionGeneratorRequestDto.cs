using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.GlobalizationResources;
using System.ComponentModel.DataAnnotations;
using AxiteHR.Services.InvoiceAPI.Models.Enums;
using AxiteHR.Services.InvoiceAPI.Attributes;

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
	}
}
