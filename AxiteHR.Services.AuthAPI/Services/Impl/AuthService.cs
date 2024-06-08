using AxiteHR.GlobalizationResources;
using AxiteHR.Services.AuthAPI.Data;
using AxiteHR.Services.AuthAPI.Models;
using AxiteHR.Services.AuthAPI.Models.Const;
using AxiteHR.Services.AuthAPI.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace AxiteHR.Services.AuthAPI.Services.Impl
{
	public class AuthService : IAuthService
	{
		private readonly AppDbContext _dbContext;
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IJwtTokenGenerator _jwtTokenGenerator;
		private readonly IStringLocalizer<AuthResources> _authLocalizer;

		public AuthService(AppDbContext dbContext,
			UserManager<AppUser> userManager,
			RoleManager<IdentityRole> roleManager,
			IJwtTokenGenerator jwtTokenGenerator,
			IStringLocalizer<AuthResources> authLocalizer)
		{
			_dbContext = dbContext;
			_userManager = userManager;
			_roleManager = roleManager;
			_jwtTokenGenerator = jwtTokenGenerator;
			_authLocalizer = authLocalizer;
		}

		public async Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
		{
			var user = _dbContext.AppUserList.FirstOrDefault(x => x.Email.ToLower() == loginRequest.Email.ToLower());
			if (user == null)
			{
				return new LoginResponseDto
				{
					IsLoggedSuccessful = false,
					ErrorMessage = _authLocalizer[AuthResourcesGenerated.LoginInvalidData]
				};
			}

			var isValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
			if (!isValid)
			{
				return new LoginResponseDto
				{
					IsLoggedSuccessful = false,
					ErrorMessage = "Invalid mail or password"
				};
			}

			var token = _jwtTokenGenerator.GenerateToken(user);

			return new LoginResponseDto
			{
				IsLoggedSuccessful = true,
				Token = token
			};
		}

		public async Task<RegisterResponseDto> Register(RegisterRequestDto registerRequest)
		{
			var isUserMailInDb = _dbContext.AppUserList.FirstOrDefault(x => x.Email.ToLower() == registerRequest.Email.ToLower());
			if (isUserMailInDb != null)
			{
				var response = new RegisterResponseDto
				{
					IsRegisteredSuccessful = false,
					ErrorMessage = _authLocalizer[AuthResourcesGenerated.RegisterEmailExistsInDb]
				};
				return response;
			}

			var isUserNameInDb = _dbContext.AppUserList.FirstOrDefault(x => x.UserName!.ToLower() == registerRequest.UserName.ToLower());
			if (isUserNameInDb != null)
			{
				var response = new RegisterResponseDto
				{
					IsRegisteredSuccessful = false,
					ErrorMessage = _authLocalizer[AuthResourcesGenerated.RegisterUserNameExistsInDb]
				};
				return response;
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

			var transaction = _dbContext.Database.BeginTransaction();
			try
			{
				var result = await _userManager.CreateAsync(user, registerRequest.UserPassword);
				if (!result.Succeeded)
				{
					return new RegisterResponseDto
					{
						IsRegisteredSuccessful = false,
						ErrorMessage = _authLocalizer[AuthResourcesGenerated.RegisterGlobalError]
					};
				}

				var isRoleAssigned = await AssignRoleAfterRegistration(user, Roles.User);
				if (!isRoleAssigned)
				{
					var response = new RegisterResponseDto
					{
						IsRegisteredSuccessful = false
					};
					return response;
				}

				_dbContext.SaveChanges();
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
				var response = new RegisterResponseDto
				{
					IsRegisteredSuccessful = false,
					ErrorMessage = _authLocalizer[AuthResourcesGenerated.RegisterGlobalError]
				};
				return response;
			}
			throw new NotImplementedException();

			async Task<bool> AssignRoleAfterRegistration(AppUser user, string roleName)
			{
				var isRoleExists = await _roleManager.RoleExistsAsync(roleName);
				if (!isRoleExists)
				{
					return false;
				}
				await _userManager.AddToRoleAsync(user, roleName);
				return true;
			}
		}
	}
}