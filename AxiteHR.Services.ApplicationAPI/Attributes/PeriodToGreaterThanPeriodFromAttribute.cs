using System.ComponentModel.DataAnnotations;

namespace AxiteHR.Services.ApplicationAPI.Attributes;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class PeriodToGreaterThanPeriodFromAttribute(string comparisonProperty) : ValidationAttribute
{
	private readonly string _comparisonProperty = comparisonProperty;

	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		var currentValue = value as DateTime?;

		var property = validationContext.ObjectType.GetProperty(_comparisonProperty)
			?? throw new ArgumentException($"Property with name {_comparisonProperty} not found.");

		var comparisonValue = (DateTime?)property.GetValue(validationContext.ObjectInstance);

		if (currentValue.HasValue && comparisonValue.HasValue && currentValue <= comparisonValue)
		{
			return new ValidationResult($"'{validationContext.DisplayName}' must be greater than '{_comparisonProperty}'.");
		}

		return ValidationResult.Success;
	}
}