using AxiteHR.GlobalizationResources.Resources;
using System.ComponentModel.DataAnnotations;

namespace AxiteHr.Services.CompanyAPI.Models.Dto.Request
{
	public class NewCompanyRequestDto
	{
		[Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "Global_RequiredField")]
		public string CompanyName { get; set; } = string.Empty;

		public Guid CreatorId { get; set; }
	}
}
