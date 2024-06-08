using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AxiteHr.Services.CompanyAPI.Models
{
	public class CompanyRole
	{
		[Key]
		public virtual int Id { get; set; }

		public virtual string RoleName { get; set; } = string.Empty;

		public virtual bool IsMain { get; set; }
	}
}
