using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using System.ComponentModel.DataAnnotations;

namespace AxiteHR.Services.CompanyAPI.Models.EmployeeModels.Dto
{
	public record NewEmployeeRequestDto
	{
		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[Display(Name = CompanyResourcesKeys.NewEmployeeRequestDto_CompanyId, ResourceType = typeof(CompanyResources))]
		public int CompanyId { get; set; }

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[EmailAddress(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_InvalidEmailAddress)]
		[Display(Name = CompanyResourcesKeys.NewEmployeeRequestDto_Email, ResourceType = typeof(CompanyResources))]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[MinLength(5, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_MinLengthField)]
		[Display(Name = CompanyResourcesKeys.NewEmployeeRequestDto_UserName, ResourceType = typeof(CompanyResources))]
		public string UserName { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[MinLength(2, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_MinLengthField)]
		[Display(Name = CompanyResourcesKeys.NewEmployeeRequestDto_FirstName, ResourceType = typeof(CompanyResources))]
		public string FirstName { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[MinLength(2, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_MinLengthField)]
		[Display(Name = CompanyResourcesKeys.NewEmployeeRequestDto_LastName, ResourceType = typeof(CompanyResources))]
		public string LastName { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[Display(Name = SharedResourcesKeys.Global_InsUserId, ResourceType = typeof(SharedResources))]
		public string InsUserId { get; set; } = string.Empty;
	}
}
