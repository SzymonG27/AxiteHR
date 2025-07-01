using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AxiteHR.Services.InvoiceAPI.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public class PostalCodeAttribute : ValidationAttribute
	{
		private static readonly Regex _regex = new Regex(@"^\d{2}-\d{3}$");

		public override bool IsValid(object? value)
		{
			if (value == null)
				return true;

			var code = value as string;

			if (string.IsNullOrWhiteSpace(code))
				return true;

			return _regex.IsMatch(code);
		}
	}
}
