using System.ComponentModel.DataAnnotations;

namespace AxiteHR.Services.ApplicationAPI.Attributes;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class PeriodToGreaterThanPeriodFromAttribute(string comparisonProperty) : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		var currentValue = value as DateTime?;

		var property = validationContext.ObjectType.GetProperty(comparisonProperty)
			?? throw new ArgumentException($"Property with name {comparisonProperty} not found.");

		var comparisonValue = (DateTime?)property.GetValue(validationContext.ObjectInstance);

		if (currentValue <= comparisonValue)
		{
			return new ValidationResult($"'{validationContext.DisplayName}' must be greater than '{comparisonProperty}'.");
		}

		return ValidationResult.Success;
	}
}