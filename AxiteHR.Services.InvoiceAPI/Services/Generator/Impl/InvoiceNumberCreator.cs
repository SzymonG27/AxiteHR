using AxiteHR.Services.InvoiceAPI.Helpers;
using AxiteHR.Services.InvoiceAPI.Models.Enums;

namespace AxiteHR.Services.InvoiceAPI.Services.Generator.Impl
{
	public static class InvoiceNumberCreator
	{
		public static string GetInvoiceNumber(InvoiceType type, int year, int month, int currentNumber)
		{
			var shortcut = GetInvoiceShortcut(type);

			return $"{shortcut}/{year:D4}/{month:D2}/{currentNumber:D5}";
		}

		private static string GetInvoiceShortcut(InvoiceType type)
		{
			switch (type)
			{
				case InvoiceType.Invoice:
					return InvoiceShortcutsHelper.Invoice;
				case InvoiceType.CorrectiveInvoice:
					return InvoiceShortcutsHelper.CorrectiveInvoice;
				default:
					return "";
			}
		}
	}
}
