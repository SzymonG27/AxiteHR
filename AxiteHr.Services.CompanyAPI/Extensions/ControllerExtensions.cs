using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AxiteHR.Services.CompanyAPI.Extensions
{
	/// <summary>
	/// Provides extension methods for <see cref="ControllerBase"/> to simplify creation of standardized responses.
	/// </summary>
	public static class ControllerExtensions
	{
		/// <summary>
		/// Creates an <see cref="IActionResult"/> representing an error, with the given HTTP status code and message.
		/// </summary>
		/// <param name="ctrl">
		/// The controller instance from which this extension method is invoked.
		/// </param>
		/// <param name="code">
		/// The <see cref="HttpStatusCode"/> to use for the response.
		/// </param>
		/// <param name="message">
		/// A human-readable error message to include in the response body.
		/// </param>
		/// <returns>
		/// An <see cref="IActionResult"/> whose status code is set to <paramref name="code"/> and whose body
		/// contains a JSON object with an <c>error</c> property set to <paramref name="message"/>.
		/// </returns>
		public static IActionResult Error(this ControllerBase ctrl, HttpStatusCode? code, string? message)
			=> ctrl.StatusCode((int?)code ?? (int)HttpStatusCode.InternalServerError, new { error = message ?? string.Empty });
	}
}
