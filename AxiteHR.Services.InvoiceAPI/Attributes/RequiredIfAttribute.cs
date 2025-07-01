using System.ComponentModel.DataAnnotations;

namespace AxiteHR.Services.InvoiceAPI.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public class RequiredIfAttribute : ValidationAttribute
	{
		private readonly string _dependentProperty;
		private readonly object _targetValue;

		public RequiredIfAttribute(string dependentProperty, object targetValue)
		{
			_dependentProperty = dependentProperty;
			_targetValue = targetValue;
		}

		protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
		{
			var containerType = validationContext.ObjectInstance.GetType();
			var field = containerType.GetProperty(_dependentProperty);
			if (field == null)
			{
				return new ValidationResult($"Unknown property: {_dependentProperty}");
			}

			var dependentValue = field.GetValue(validationContext.ObjectInstance, null);

			if (Equals(dependentValue, _targetValue) && value is string str && string.IsNullOrWhiteSpace(str))
			{
				var displayName = validationContext.DisplayName ?? validationContext.MemberName;
				var message = FormatErrorMessage(displayName ?? " ");
				return new ValidationResult(message);
			}

			return ValidationResult.Success!;
		}
	}
}