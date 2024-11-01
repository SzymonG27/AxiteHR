using AxiteHR.Integration.GlobalClass.Auth;
using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Services.ApplicationAPI.Helpers;
using AxiteHR.Services.ApplicationAPI.Models.Application.Dto;
using AxiteHR.Services.ApplicationAPI.Services.Application;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AxiteHR.Services.ApplicationAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApplicationController(
		IApplicationService applicationService,
		IStringLocalizer<SharedResources> sharedLocalizer) : ControllerBase
	{
		/// <summary>
		/// Handles the HTTP POST request to create a new user application.
		/// The method validates the request data, checks for overlapping application periods,
		/// and ensures the user has enough available days off before creating the application.
		/// </summary>
		/// <param name="createApplicationRequestDto">The DTO containing the details of the application request, including period, type, and user information.</param>
		/// <param name="acceptLanguage">Accept language from header as two-letter code</param>
		/// <returns>
		/// Returns an <see cref="IActionResult"/>:
		/// <list type="bullet">
		/// <item><description><see cref="BadRequestObjectResult"/> if the application creation fails, returning the error details in the response.</description></item>
		/// <item><description><see cref="OkObjectResult"/> if the application is created successfully.</description></item>
		/// </list>
		/// </returns>
		/// <remarks>
		/// This method interacts with the <c>applicationService</c> to process the creation logic and responds with the appropriate HTTP status code based on the result of the operation.
		/// </remarks>
		[HttpPost("[action]")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.UserFromCompany)]
		public async Task<IActionResult> CreateNewApplication(
			[FromBody] CreateApplicationRequestDto createApplicationRequestDto,
			[FromHeader(Name = HeaderNamesHelper.AcceptLanguage)] string acceptLanguage = "en")
		{
			var bearerToken = await HttpContext.GetTokenAsync(HeaderNamesHelper.AccessTokenContext);
			if (string.IsNullOrEmpty(bearerToken))
			{
				return Unauthorized(sharedLocalizer[SharedResourcesKeys.Global_MissingToken]);
			}

			var response = await applicationService.CreateNewUserApplicationAsync(createApplicationRequestDto, bearerToken, acceptLanguage);
			if (!response.IsSucceeded)
			{
				return BadRequest(response);
			}

			return Ok();
		}
	}
}
