using System.ComponentModel.DataAnnotations;
using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;

namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request
{
	public record NewCompanyRequestDto
	{
		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		public string CompanyName { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		public Guid CreatorId { get; set; }

		public Guid InsUserId => CreatorId;

		public Guid UpdUserId => CreatorId;

		public static DateTime InsDate => DateTime.UtcNow;

		public static DateTime UpdDate => DateTime.UtcNow;
	}
}