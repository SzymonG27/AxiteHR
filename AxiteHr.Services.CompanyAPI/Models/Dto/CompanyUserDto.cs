using System.ComponentModel.DataAnnotations.Schema;

namespace AxiteHr.Services.CompanyAPI.Models.Dto
{
	public class CompanyUserDto
	{
		public int Id { get; set; }

		public int CompanyId { get; set; }

		public CompanyDto Company { get; set; } = new CompanyDto();

		public Guid UserId { get; set; }
	}
}
