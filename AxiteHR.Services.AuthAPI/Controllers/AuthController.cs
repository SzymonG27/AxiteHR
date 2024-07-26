using AxiteHR.Services.AuthAPI.Models.Auth.Const;
using AxiteHR.Services.AuthAPI.Models.Auth.Dto;
using AxiteHR.Services.AuthAPI.Models.EmployeeModels.Dto;
using AxiteHR.Services.AuthAPI.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
				return BadRequest(response);
			}
			return Ok();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
		{
			var response = await authService.Login(loginRequest);
			if (!response.IsLoggedSuccessful)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}

		[HttpPost("[action]")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.Admin},{Roles.User}")]
		public async Task<IActionResult> RegisterNewEmployee([FromBody] NewEmployeeRequestDto newEmployeeRequestDto)
		{
			var response = await authService.RegisterNewEmployee(newEmployeeRequestDto);
			if (!response.IsSucceeded)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}

		[HttpPost("[action]")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.Admin},{Roles.User}")]
		public async Task<IActionResult> TempPasswordChange([FromBody] TempPasswordChangeRequestDto tempPasswordChangeDto)
		{
			var response = await authService.TempPasswordChange(tempPasswordChangeDto);
			if (!response.IsSucceeded)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}
	}
}
