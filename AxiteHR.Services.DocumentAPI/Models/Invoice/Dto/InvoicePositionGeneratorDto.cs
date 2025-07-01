using AxiteHR.GlobalizationResources;
using AxiteHR.Integration.GlobalClass.Enums;
using AxiteHR.Integration.GlobalClass.Enums.Invoice;

namespace AxiteHR.Services.DocumentAPI.Models.Invoice.Dto
{
	public record InvoicePositionGeneratorDto
	{
		public string ProductName { get; set; } = string.Empty;

		public Unit Unit { get; set; }

		public decimal Quantity { get; set; }

		public decimal NetPrice { get; set; }

		public int VatRate { get; set; }

		public decimal VatAmount { get; set; }

		public decimal NetAmount { get; set; }

		public decimal GrossAmount { get; set; }

		public string GetUnitTranslationPositionString()
		{
			return Unit switch
			{
				Unit.Piece => DocumentResourcesKeys.Document_InvoicePosition_Unit_Piece,
				Unit.Hour => DocumentResourcesKeys.Document_InvoicePosition_Unit_Hour,
				_ => string.Empty,
			};
		}
	}
}
