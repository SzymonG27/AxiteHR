using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Services.ApplicationAPI.Attributes;
using AxiteHR.Services.ApplicationAPI.Models.Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace AxiteHR.Services.ApplicationAPI.Models.Application.Dto
{
	public record CreateApplicationRequestDto
	{
		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[Display(ResourceType = typeof(ApplicationResources), Name = ApplicationResourcesKeys.CreateApplication_CompanyUserId)]
		public int CompanyUserId { get; set; }

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[Display(ResourceType = typeof(ApplicationResources), Name = ApplicationResourcesKeys.CreateApplication_ApplicationType)]
		public ApplicationType ApplicationType { get; set; }

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[Display(ResourceType = typeof(SharedResources), Name = SharedResourcesKeys.Global_PeriodFrom)]
		public DateTime PeriodFrom { get; set; }

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[PeriodToGreaterThanPeriodFrom(nameof(PeriodFrom), ErrorMessageResourceType = typeof(ApplicationResources), ErrorMessageResourceName = ApplicationResourcesKeys.CreateApplication_PeriodToGreaterThanPeriodFrom)]
		[Display(ResourceType = typeof(SharedResources), Name = SharedResourcesKeys.Global_PeriodTo)]
		public DateTime PeriodTo { get; set; }

		[Display(ResourceType = typeof(ApplicationResources), Name = ApplicationResourcesKeys.CreateApplication_ApplicationReason)]
		public string? Reason { get; set; }

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		[Display(ResourceType = typeof(SharedResources), Name = SharedResourcesKeys.Global_InsUserId)]
		public Guid InsUserId { get; set; }
	}
}
