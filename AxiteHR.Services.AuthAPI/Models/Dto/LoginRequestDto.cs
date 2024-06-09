using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using System.ComponentModel.DataAnnotations;

namespace AxiteHR.Services.AuthAPI.Models.Dto
{
	public class LoginRequestDto
	{
		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[EmailAddress(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_InvalidEmailAddress)]
		[Display(Name = AuthResourcesKeys.Model_Email, ResourceType = typeof(AuthResources))]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[Display(Name = AuthResourcesKeys.Model_Password, ResourceType = typeof(AuthResources))]
		public string Password { get; set; } = string.Empty;
	}
}