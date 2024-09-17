using Microsoft.AspNetCore.Mvc;

namespace AxiteHR.Services.ApplicationAPI.Controllers
{
	public class CalendarController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
