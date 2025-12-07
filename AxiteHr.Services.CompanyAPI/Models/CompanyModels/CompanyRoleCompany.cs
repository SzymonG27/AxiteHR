namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels
{
	public class CompanyRoleCompany
	{
		public virtual int Id { get; set; }

		public virtual int CompanyId { get; set; }

		public virtual Company Company { get; set; } = new();

		public virtual int CompanyRoleId { get; set; }

		public virtual CompanyRole CompanyRole { get; set; } = new();

		public virtual bool IsMain { get; set; }

		public virtual bool IsVisible { get; set; }
	}
}
