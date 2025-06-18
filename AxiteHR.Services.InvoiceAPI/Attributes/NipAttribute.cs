using System.ComponentModel.DataAnnotations;

namespace AxiteHR.Services.InvoiceAPI.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public class NipAttribute : ValidationAttribute
	{
		public override bool IsValid(object? value)
		{
			if (value == null)
				return true;

			string? nip = (value as string)?.Replace("-", "").Replace(" ", "");

			if (string.IsNullOrEmpty(nip))
				return true;

			if (nip.Length != 10 || !nip.All(char.IsDigit))
				return false;

			int[] weights = { 6, 5, 7, 2, 3, 4, 5, 6, 7 };
			int sum = weights.Select((w, i) => w * (nip[i] - '0')).Sum();
			int control = sum % 11;
			if (control == 10) control = 0;

			return control == (nip[9] - '0');
		}
	}
}
