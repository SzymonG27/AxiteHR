using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Services.AuthAPI.Data;
using AxiteHR.Services.AuthAPI.Models.Auth;
using AxiteHR.Services.AuthAPI.Models.Auth.Const;
using AxiteHR.Services.AuthAPI.Models.Auth.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace AxiteHR.Services.AuthAPI.Services.Auth.Impl
{
	public class AuthService(AppDbContext dbContext,
		UserManager<AppUser> userManager,
		RoleManager<IdentityRole> roleManager,
		IJwtTokenGenerator jwtTokenGenerator,
		IStringLocalizer<AuthResources> authLocalizer) : IAuthService
	{
		public async Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
		{
			var user = dbContext.AppUserList.FirstOrDefault(x => x.Email == loginRequest.Email);
			if (user == null)
			{
				return new LoginResponseDto
				{
					IsLoggedSuccessful = false,
					ErrorMessage = authLocalizer[AuthResourcesKeys.LoginInvalidData]
				};
			}

			var isValid = await userManager.CheckPasswordAsync(user, loginRequest.Password);
			if (!isValid)
			{
				return new LoginResponseDto
				{
					IsLoggedSuccessful = false,
					ErrorMessage = "Invalid mail or password"
				};
			}

			var roleList = await userManager.GetRolesAsync(user);
			var token = jwtTokenGenerator.GenerateToken(user, roleList);

			return new LoginResponseDto
			{
				IsLoggedSuccessful = true,
				Token = token
			};
		}

		public async Task<RegisterResponseDto> Register(RegisterRequestDto registerRequest)
		{
			var isUserMailInDb = dbContext.AppUserList.FirstOrDefault(x => x.Email == registerRequest.Email);
			if (isUserMailInDb != null)
			{
				return new RegisterResponseDto
				{
					IsRegisteredSuccessful = false,
					ErrorMessage = authLocalizer[AuthResourcesKeys.RegisterEmailExistsInDb]
				};
			}

			var isUserNameInDb = dbContext.AppUserList.FirstOrDefault(x => x.UserName == registerRequest.UserName);
			if (isUserNameInDb != null)
			{
				return new RegisterResponseDto
				{
					IsRegisteredSuccessful = false,
					ErrorMessage = authLocalizer[AuthResourcesKeys.RegisterUserNameExistsInDb]
				};
			}

			var user = new AppUser
			{
				Email = registerRequest.Email,
				NormalizedEmail = registerRequest.Email.ToUpper(),
				UserName = registerRequest.UserName,
				NormalizedUserName = registerRequest.UserName.ToUpper(),
				FirstName = registerRequest.FirstName,
				LastName = registerRequest.LastName,
				PhoneNumber = registerRequest.PhoneNumber
			};

			var transaction = dbContext.Database.BeginTransaction();
			try
			{
				var result = await userManager.CreateAsync(user, registerRequest.UserPassword);
				if (!result.Succeeded)
				{
					return new RegisterResponseDto
					{
						IsRegisteredSuccessful = false,
						ErrorMessage = authLocalizer[AuthResourcesKeys.RegisterGlobalError]
					};
				}

				var isRoleAssigned = await AssignRoleAfterRegistration(user, Roles.User);
				if (!isRoleAssigned)
				{
					return new RegisterResponseDto
					{
						IsRegisteredSuccessful = false
					};
				}

				dbContext.SaveChanges();
				transaction.Commit();
				return new RegisterResponseDto
				{
					IsRegisteredSuccessful = true
				};
			}
			catch (Exception)
			{
				//ToDo logger error app insights
				transaction.Rollback();
				return new RegisterResponseDto
				{
					IsRegisteredSuccessful = false,
					ErrorMessage = authLocalizer[AuthResourcesKeys.RegisterGlobalError]
				};
			}
			throw new NotImplementedException();

			async Task<bool> AssignRoleAfterRegistration(AppUser user, string roleName)
			{
				var isRoleExists = await roleManager.RoleExistsAsync(roleName);
				if (!isRoleExists)
				{
					return false;
				}
				await userManager.AddToRoleAsync(user, roleName);
				return true;
			}
		}
	}
}