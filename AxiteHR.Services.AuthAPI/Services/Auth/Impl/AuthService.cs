using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Services.AuthAPI.Data;
using AxiteHR.Services.AuthAPI.Helpers;
using AxiteHR.Services.AuthAPI.Models.Auth;
using AxiteHR.Services.AuthAPI.Models.Auth.Const;
using AxiteHR.Services.AuthAPI.Models.Auth.Dto;
using AxiteHR.Services.AuthAPI.Models.EmployeeModels.Dto;
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
			return await RegisterUser(registerRequest, registerRequest.UserPassword, Roles.User, false,
				(user, isSuccess, errorMessage) => new RegisterResponseDto
				{
					IsRegisteredSuccessful = isSuccess,
					ErrorMessage = errorMessage,
					UserId = user?.Id ?? string.Empty
				});
		}

		public async Task<NewEmployeeResponseDto> RegisterNewEmployee(NewEmployeeRequestDto newEmployeeRequestDto)
		{
			var tempPassword = TempPasswordHelper.GenerateTempPassword();
			return await RegisterUser(newEmployeeRequestDto, tempPassword, Roles.UserFromCompany, true,
				(user, isSuccess, errorMessage) => new NewEmployeeResponseDto
				{
					IsSucceeded = isSuccess,
					ErrorMessage = errorMessage,
					EmployeeId = user?.Id ?? string.Empty
				});
		}

		#region Private Methods
		private async Task<TResponse> RegisterUser<TRequest, TResponse>(TRequest request, string password, string role, bool isTempPassword, Func<AppUser?, bool, string, TResponse> createResponse)
		{
			string email, userName, firstName, lastName, phoneNumber = string.Empty;
			switch (request)
			{
				case RegisterRequestDto regReq:
					email = regReq.Email;
					userName = regReq.UserName;
					firstName = regReq.FirstName;
					lastName = regReq.LastName;
					phoneNumber = regReq.PhoneNumber;
					break;
				case NewEmployeeRequestDto empReq:
					email = empReq.Email;
					userName = empReq.UserName;
					firstName = empReq.FirstName;
					lastName = empReq.LastName;
					break;
				default:
					throw new ArgumentException("Invalid request type");
			}

			if (!IsUserMailValidateSucceeded(email))
			{
				return createResponse(null, false, authLocalizer[AuthResourcesKeys.RegisterEmailExistsInDb]);
			}

			if (!IsUserNameValidateSucceeded(userName))
			{
				return createResponse(null, false, authLocalizer[AuthResourcesKeys.RegisterUserNameExistsInDb]);
			}

			var user = new AppUser
			{
				Email = email,
				NormalizedEmail = email.ToUpper(),
				UserName = userName,
				NormalizedUserName = userName.ToUpper(),
				FirstName = firstName,
				LastName = lastName,
				PhoneNumber = phoneNumber,
				IsTempPassword = isTempPassword
			};

			await using var transaction = await dbContext.Database.BeginTransactionAsync();
			try
			{
				var result = await userManager.CreateAsync(user, password);
				if (!result.Succeeded)
				{
					return createResponse(null, false, authLocalizer[AuthResourcesKeys.RegisterGlobalError]);
				}

				var isRoleAssigned = await AssignRoleAfterRegistration(user, role);
				if (!isRoleAssigned)
				{
					return createResponse(null, false, authLocalizer[AuthResourcesKeys.RegisterGlobalError]);
				}

				await dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
				return createResponse(user, true, string.Empty);
			}
			catch (Exception)
			{
				// ToDo logger error app insights
				await transaction.RollbackAsync();
				return createResponse(null, false, authLocalizer[AuthResourcesKeys.RegisterGlobalError]);
			}
		}

		private async Task<bool> AssignRoleAfterRegistration(AppUser user, string roleName)
		{
			var isRoleExists = await roleManager.RoleExistsAsync(roleName);
			if (!isRoleExists)
			{
				return false;
			}
			await userManager.AddToRoleAsync(user, roleName);
			return true;
		}

		private bool IsUserMailValidateSucceeded(string email)
		{
			var isUserMailInDb = dbContext.AppUserList.FirstOrDefault(x => x.Email == email);
			return isUserMailInDb == null;
		}

		private bool IsUserNameValidateSucceeded(string userName)
		{
			var isUserNameInDb = dbContext.AppUserList.FirstOrDefault(x => x.UserName == userName);
			return isUserNameInDb == null;
		}
		#endregion
	}
}