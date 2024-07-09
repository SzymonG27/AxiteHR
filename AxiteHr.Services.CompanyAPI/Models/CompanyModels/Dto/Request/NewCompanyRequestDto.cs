using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Const;
using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using System.ComponentModel.DataAnnotations;

namespace AxiteHr.Services.CompanyAPI.CompanyModels.Dto.Request
{
	public record NewCompanyRequestDto
	{
		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		public string CompanyName { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = SharedResourcesKeys.Global_RequiredField)]
		public Guid CreatorId { get; set; }

		public Guid InsUserId => CreatorId;

		public Guid UpdUserId => CreatorId;

		public DateTime InsDate => DateTime.UtcNow;

		public DateTime UpdDate => DateTime.UtcNow;
	}
}