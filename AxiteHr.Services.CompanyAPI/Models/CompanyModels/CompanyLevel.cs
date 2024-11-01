using System.ComponentModel.DataAnnotations;

namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels
{
	/// <summary>
	/// Specifies subscription for company
	/// </summary>
	public class CompanyLevel
	{
		[Key]
		public virtual int Id { get; set; }

		public virtual int MaxNumberOfWorkers { get; set; }
	}
}
