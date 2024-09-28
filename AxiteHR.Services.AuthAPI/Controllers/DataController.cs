using AxiteHr.Integration.GlobalClass.Auth;
using AxiteHR.Services.AuthAPI.Models.DataModels.Dto;
using AxiteHR.Services.AuthAPI.Services.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AxiteHR.Services.AuthAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DataController(IDataService dataService) : ControllerBase
	{
		/// <summary>
		/// This method is used to populate the company's employee list view data.
		/// Used POST because list of user ids can be big.
		/// </summary>
		/// <param name="userIds">List of user ids</param>
		/// <returns>List of company's employee list view data</returns>
		[HttpPost("[action]")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.Admin},{Roles.User}")]
		public IEnumerable<UserDataListViewDto> GetUserDataListViews([FromBody]IList<string> userIds)
		{
			return dataService.GetUserDataListViewDtoList(userIds);
		}
	}
}
