using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AxiteHr.Services.CompanyAPI.Models.Dto
{
	public class CompanyDto
	{
		public int Id { get; set; }

		public string CompanyName { get; set; } = string.Empty;

		public int CompanyLevelId { get; set; }

		public CompanyLevelDto CompanyLevel { get; set; } = new CompanyLevelDto();
	}
}
