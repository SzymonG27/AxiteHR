using Microsoft.AspNetCore.Mvc;

namespace AxiteHR.Services.ApplicationAPI.Controllers
{
	public class ApplicationController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
