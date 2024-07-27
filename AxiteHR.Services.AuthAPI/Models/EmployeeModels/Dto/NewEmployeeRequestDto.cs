using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using System.ComponentModel.DataAnnotations;

namespace AxiteHR.Services.AuthAPI.Models.EmployeeModels.Dto
{
	public record NewEmployeeRequestDto
	{
		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[Display(Name = AuthResourcesKeys.Model_CompanyId, ResourceType = typeof(AuthResources))]
		public int CompanyId { get; set; }

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
	}
}
