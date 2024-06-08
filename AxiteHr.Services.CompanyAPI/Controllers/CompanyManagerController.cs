using Microsoft.AspNetCore.Mvc;

namespace AxiteHr.Services.CompanyAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CompanyManagerController : ControllerBase
	{
		public IActionResult Index()
		{
			return Ok();
		}
	}
}
