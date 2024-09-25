using AxiteHR.Services.ApplicationAPI.Models.Application.Dto;

namespace AxiteHR.Services.ApplicationAPI.Services.Application
{
	public interface IApplicationService
	{
		/// <summary>
		/// Asynchronously creates a new user application, validating if the requested application period 
		/// intersects with any existing application periods or if the user has sufficient available days off. 
		/// If the validation passes, the application is saved in the database and the number of available 
		/// days off for the user is updated.
		/// </summary>
		/// <param name="createApplicationRequestDto">The DTO containing the details of the application request, including the period and type of application.</param>
		/// <returns>
		/// A <see cref="CreateApplicationResponseDto"/> indicating whether the operation succeeded or failed.
		/// If the operation fails due to an intersection with another application period or insufficient available days off, 
		/// the response will contain an appropriate error message.
		/// </returns>
		/// <remarks>
		/// The method operates within a database transaction to ensure that either all changes (creating the application and updating 
		/// the user's days off) are committed or none in case of a failure. The transaction is rolled back in case of any exceptions.
		/// </remarks>
		Task<CreateApplicationResponseDto> CreateNewUserApplicationAsync(CreateApplicationRequestDto createApplicationRequestDto);
	}
}
