using AxiteHR.Services.AuthAPI.Models.Auth.Dto;
using AxiteHR.Services.AuthAPI.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace AxiteHR.Services.AuthAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController(IAuthService authService) : ControllerBase
	{
		[HttpPost("[action]")]
		public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
		{
			var response = await authService.Register(registerRequest);
			if (!response.IsRegisteredSuccessful)
			{
				return BadRequest(new BadRequestObjectResult(response));
			}
			return Ok();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
		{
			var response = await authService.Login(loginRequest);
			if (!response.IsLoggedSuccessful)
			{
				return BadRequest(new BadRequestObjectResult(response));
			}
			return Ok(new OkObjectResult(response));
		}
	}
}
