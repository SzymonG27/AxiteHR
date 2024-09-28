using AxiteHr.Integration.GlobalClass.Auth;
using AxiteHR.Services.ApplicationAPI.Models.Application.Dto;
using AxiteHR.Services.ApplicationAPI.Services.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AxiteHR.Services.ApplicationAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApplicationController(IApplicationService applicationService) : ControllerBase
	{
		/// <summary>
		/// Handles the HTTP POST request to create a new user application.
		/// The method validates the request data, checks for overlapping application periods, 
		/// and ensures the user has enough available days off before creating the application.
		/// </summary>
		/// <param name="createApplicationRequestDto">The DTO containing the details of the application request, including period, type, and user information.</param>
		/// <returns>
		/// Returns an <see cref="IActionResult"/>:
		/// <list type="bullet">
		/// <item><description><see cref="BadRequest(object)"/> if the application creation fails, returning the error details in the response.</description></item>
		/// <item><description><see cref="Ok()"/> if the application is created successfully.</description></item>
		/// </list>
		/// </returns>
		/// <remarks>
		/// This method interacts with the <c>applicationService</c> to process the creation logic and responds with the appropriate HTTP status code based on the result of the operation.
		/// </remarks>
		[HttpPost("[action]")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.UserFromCompany)]
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
