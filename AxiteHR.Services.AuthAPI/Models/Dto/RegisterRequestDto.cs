using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using System.ComponentModel.DataAnnotations;

namespace AxiteHR.Services.AuthAPI.Models.Dto
{
	public class RegisterRequestDto
	{
		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[EmailAddress(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_InvalidEmailAddress)]
		[Display(Name = AuthResourcesKeys.Model_Email, ResourceType = typeof(AuthResources))]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[MinLength(5, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_MinLengthField)]
		[Display(Name = AuthResourcesKeys.Model_UserName, ResourceType = typeof(AuthResources))]
		public string UserName { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[MinLength(2, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_MinLengthField)]
		[Display(Name = AuthResourcesKeys.Model_FirstName, ResourceType = typeof(AuthResources))]
		public string FirstName { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[MinLength(2, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_MinLengthField)]
		[Display(Name = AuthResourcesKeys.Model_LastName, ResourceType = typeof(AuthResources))]
		public string LastName { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[MinLength(8, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_MinLengthField)]
		[Display(Name = AuthResourcesKeys.Model_Password, ResourceType = typeof(AuthResources))]
		public string UserPassword { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[Compare(nameof(UserPassword), ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_MinLengthField)]
		[Display(Name = AuthResourcesKeys.Model_UserPasswordRepeated, ResourceType = typeof(AuthResources))]
		public string UserPasswordRepeated { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[Display(Name = AuthResourcesKeys.Model_PhoneNumber, ResourceType = typeof(AuthResources))]
		public string PhoneNumber { get; set; } = string.Empty;
	}
}
