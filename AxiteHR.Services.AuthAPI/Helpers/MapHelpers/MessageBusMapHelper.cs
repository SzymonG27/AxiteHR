using AxiteHR.Services.AuthAPI.Models.Auth;
using AxiteHR.Services.AuthAPI.Models.Auth.Dto;

namespace AxiteHR.Services.AuthAPI.Helpers.MapHelpers
{
	public static class MessageBusMapHelper
	{
		public static UserMessageBusDto MapAppUserToUserMessageBusDto(AppUser user, string tempPassword)
		{
			//ToDo TempPassword encryption and decryption
			return new UserMessageBusDto
			{
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				UserName = user.UserName ?? "",
				UserId = user.Id,
				TempPassword = tempPassword
			};
		}
	}
}
