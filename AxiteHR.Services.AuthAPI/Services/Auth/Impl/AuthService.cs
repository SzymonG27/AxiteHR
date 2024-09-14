using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Integration.BrokerMessageSender;
using AxiteHR.Integration.BrokerMessageSender.Models;
using AxiteHR.Security.Encryption;
using AxiteHR.Services.AuthAPI.Data;
using AxiteHR.Services.AuthAPI.Helpers;
using AxiteHR.Services.AuthAPI.Helpers.MapHelpers;
using AxiteHR.Services.AuthAPI.Models.Auth;
using AxiteHR.Services.AuthAPI.Models.Auth.Const;
using AxiteHR.Services.AuthAPI.Models.Auth.Dto;
using AxiteHR.Services.AuthAPI.Models.EmployeeModels.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Serilog;

namespace AxiteHR.Services.AuthAPI.Services.Auth.Impl
{
	public class AuthService(AppDbContext dbContext,
		UserManager<AppUser> userManager,
		RoleManager<IdentityRole> roleManager,
		IJwtTokenGenerator jwtTokenGenerator,
		IStringLocalizer<AuthResources> authLocalizer,
		IConfiguration configuration,
		ITempPasswordGeneratorService tempPasswordGeneratorService,
		IEncryptionService encryptionService,
		IServiceProvider serviceProvider,
		IOptions<RabbitMqMessageSenderConfig> rabbitMqMessageSenderConfig) : IAuthService
	{
		private readonly RabbitMqMessageSenderConfig _rabbitMqMessageSenderConfig = rabbitMqMessageSenderConfig.Value;

		public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest)
		{
			var user = await userManager.FindByEmailAsync(loginRequest.Email);
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
					ErrorMessage = authLocalizer[AuthResourcesKeys.InvalidMailOrPassword]
				};
			}

			if (user.IsTempPassword)
			{
				return new LoginResponseDto
				{
					IsTempPasswordToChange = true,
					UserId = user.Id
				};
			}

			var roleList = await userManager.GetRolesAsync(user);
			var token = jwtTokenGenerator.GenerateToken(user, roleList);

			Log.Information("User with e-mail: {UserMail} logged successfully", user.Email);
			return new LoginResponseDto
			{
				IsLoggedSuccessful = true,
				Token = token
			};
		}

		public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto registerRequest)
		{
			return await RegisterUser(registerRequest, registerRequest.UserPassword, Roles.User, false,
				(user, isSuccess, errorMessage) => new RegisterResponseDto
				{
					IsRegisteredSuccessful = isSuccess,
					ErrorMessage = errorMessage,
					UserId = user?.Id ?? string.Empty
				});
		}

		public async Task<NewEmployeeResponseDto> RegisterNewEmployeeAsync(NewEmployeeRequestDto newEmployeeRequestDto)
		{
			var tempPassword = tempPasswordGeneratorService.GenerateTempPassword();
			return await RegisterUser(newEmployeeRequestDto, tempPassword, Roles.UserFromCompany, true,
				(user, isSuccess, errorMessage) => new NewEmployeeResponseDto
				{
					IsSucceeded = isSuccess,
					ErrorMessage = errorMessage,
					EmployeeId = user?.Id ?? string.Empty
				});
		}

		public async Task<TempPasswordChangeResponseDto> TempPasswordChangeAsync(TempPasswordChangeRequestDto newPasswordChangeRequest)
		{
			var user = await userManager.FindByIdAsync(newPasswordChangeRequest.UserId.ToString());
			if (user?.IsTempPassword != true)
			{
				return new TempPasswordChangeResponseDto
				{
					IsSucceeded = false,
					ErrorMessage = authLocalizer[AuthResourcesKeys.PasswordChangeGlobalError]
				};
			}

			await using var transaction = await dbContext.Database.BeginTransactionAsync();
			try
			{
				var token = await userManager.GeneratePasswordResetTokenAsync(user);
				var result = await userManager.ResetPasswordAsync(user, token, newPasswordChangeRequest.Password);

				if (!result.Succeeded)
				{
					return new TempPasswordChangeResponseDto
					{
						IsSucceeded = false,
						ErrorMessage = authLocalizer[AuthResourcesKeys.PasswordChangeGlobalError]
					};
				}

				user.IsTempPassword = false;

				await dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
				return new TempPasswordChangeResponseDto
				{
					IsSucceeded = true,
					UserId = user.Id
				};
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Temp password change failed for user: {UserMail}", user.Email);
				await transaction.RollbackAsync();
				return new TempPasswordChangeResponseDto
				{
					IsSucceeded = false,
					ErrorMessage = authLocalizer[AuthResourcesKeys.PasswordChangeGlobalError]
				};
			}
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
					Log.Error("Invalid request type RegisterUser: {RequestType}", request?.GetType().Name);
					throw new ArgumentException("Invalid request type");
			}

			if (!await IsUserMailValidateSucceededAsync(email))
			{
				return createResponse(null, false, authLocalizer[AuthResourcesKeys.RegisterEmailExistsInDb]);
			}

			if (!await IsUserNameValidateSucceededAsync(userName))
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

				var isRoleAssigned = await AssignRoleAfterRegistrationAsync(user, role);
				if (!isRoleAssigned)
				{
					return createResponse(null, false, authLocalizer[AuthResourcesKeys.RegisterGlobalError]);
				}

				if (isTempPassword)
				{
					await PublishTempPasswordMessage(user, password);
				}

				await dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
				return createResponse(user, true, string.Empty);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Regiser failed for user: {UserMail}", user.Email);
				await transaction.RollbackAsync();
				return createResponse(null, false, authLocalizer[AuthResourcesKeys.RegisterGlobalError]);
			}
		}

		private async Task<bool> AssignRoleAfterRegistrationAsync(AppUser user, string roleName)
		{
			var isRoleExists = await roleManager.RoleExistsAsync(roleName);
			if (!isRoleExists)
			{
				return false;
			}

			var result = await userManager.AddToRoleAsync(user, roleName);
			return result.Succeeded;
		}

		private async Task PublishTempPasswordMessage(AppUser user, string password)
		{
			var encryptionKey = configuration.GetValue<string>(ConfigurationHelper.TempPasswordEncryptionKey) ??
				throw new ArgumentException($"Appsetting {ConfigurationHelper.TempPasswordEncryptionKey} isn't configured");

			var encryptedPassword = encryptionService.Encrypt(password, encryptionKey);

			var messageDto = MessageBusMapHelper.MapAppUserToUserMessageBusDto(user, encryptedPassword);

			MessageSenderModel<RabbitMqMessageSenderConfig, UserMessageBusDto> messageSenderModel = new()
			{
				Message = messageDto,
				Config = _rabbitMqMessageSenderConfig
			};
			messageSenderModel.Config.QueueName = configuration.GetValue<string>(ConfigurationHelper.EmailTempPasswordQueue)!;

			var messagePublisher = serviceProvider.GetService<MessagePublisher>() ?? throw new NotSupportedException("No such service for MessagePublisher");
			await messagePublisher.PublishMessageAsync(messageSenderModel);
		}

		private async Task<bool> IsUserMailValidateSucceededAsync(string email)
		{
			var isUserMailInDb = await userManager.FindByEmailAsync(email);
			return isUserMailInDb == null;
		}

		private async Task<bool> IsUserNameValidateSucceededAsync(string userName)
		{
			var isUserNameInDb = await userManager.FindByNameAsync(userName);
			return isUserNameInDb == null;
		}
		#endregion
	}
}