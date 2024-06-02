using AxiteHR.Services.AuthAPI.Models.Dto;
using AxiteHR.Services.AuthAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AxiteHR.Services.AuthAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}
		[HttpPost("[action]")]
		public async Task<IActionResult> Register([FromBody]RegisterRequestDto registerRequest)
		{
			var response = await _authService.Register(registerRequest);
			if (!response.IsRegisteredSuccessful)
			{
				return BadRequest(new BadRequestObjectResult(response));
			}
			return Ok();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> Login([FromBody]LoginRequestDto loginRequest)
		{
			var response = await _authService.Login(loginRequest);
			if (!response.IsLoggedSuccessful)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}
	}
}
