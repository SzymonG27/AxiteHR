using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using System.ComponentModel.DataAnnotations;

namespace AxiteHr.Services.CompanyAPI.CompanyModels.Dto.Request
{
	public record NewCompanyRequestDto
	{
		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		public string CompanyName { get; set; } = string.Empty;

		public Guid CreatorId { get; set; }
	}
}
