using System.ComponentModel.DataAnnotations;

namespace AxiteHR.Services.InvoiceAPI.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public class DecimalPrecisionAttribute : ValidationAttribute
	{
		private readonly int _decimalPlaces;

		public DecimalPrecisionAttribute(int decimalPlaces)
		{
			_decimalPlaces = decimalPlaces;
		}

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value == null)
				return ValidationResult.Success;

			if (value is decimal decimalValue)
			{
				decimalValue = Math.Abs(decimalValue);
				var places = BitConverter.GetBytes(decimal.GetBits(decimalValue)[3])[2];
				if (places > _decimalPlaces)
				{
					var errorMessage = FormatErrorMessage(validationContext.DisplayName ?? validationContext.MemberName ?? "value");
					return new ValidationResult(errorMessage);
				}
			}

			return ValidationResult.Success!;
		}

		public override string FormatErrorMessage(string name)
		{
			return string.Format(ErrorMessage ?? "{0} must have no more than {1} decimal places.", name, _decimalPlaces);
		}
	}
}
