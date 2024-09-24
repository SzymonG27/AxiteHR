using AxiteHR.Services.ApplicationAPI.Models.Application.Dto;
using AxiteHR.Services.ApplicationAPI.Services.Application;
using Microsoft.AspNetCore.Mvc;

namespace AxiteHR.Services.ApplicationAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApplicationController(IApplicationService applicationService) : ControllerBase
	{
		[HttpPost("[action]")]
		public async Task<IActionResult> CreateNewApplication([FromBody] CreateApplicationRequestDto createApplicationRequestDto)
		{
			var response = await applicationService.CreateNewUserApplicationAsync(createApplicationRequestDto);
			if (!response.IsSucceeded)
			{
				return BadRequest(response);
			}
			return Ok();
		}
	}
}
