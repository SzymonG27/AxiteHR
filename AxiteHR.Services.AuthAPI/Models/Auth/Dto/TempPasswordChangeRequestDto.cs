using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.GlobalizationResources;
using System.ComponentModel.DataAnnotations;

namespace AxiteHR.Services.AuthAPI.Models.Auth.Dto
{
	public record TempPasswordChangeRequestDto
	{
		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		public Guid UserId { get; set; }

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[MinLength(8, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_MinLengthField)]
		[Display(Name = AuthResourcesKeys.Model_Password, ResourceType = typeof(AuthResources))]
		public string Password { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[Compare(nameof(Password), ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_MinLengthField)]
		[Display(Name = AuthResourcesKeys.Model_UserPasswordRepeated, ResourceType = typeof(AuthResources))]
		public string ConfirmPassword { get; set; } = string.Empty;
	}
}
