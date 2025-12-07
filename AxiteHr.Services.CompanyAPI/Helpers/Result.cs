using System.Net;

namespace AxiteHR.Services.CompanyAPI.Helpers
{
	/// <summary>
	/// Represents the outcome of an operation, encapsulating either a successful result with a value
	/// or a failure with an error message.
	/// </summary>
	/// <typeparam name="T">The type of the value returned on success.</typeparam>
	public sealed class Result<T>
	{
		/// <summary>
		/// Gets a value indicating whether the operation succeeded.
		/// </summary>
		public bool IsSuccess { get; }

		/// <summary>
		/// Gets the value of the operation if it succeeded; otherwise, <c>null</c>.
		/// </summary>
		public T? Value { get; }

		/// <summary>
		/// Gets the error message if the operation failed; otherwise, <c>null</c>.
		/// </summary>
		public string? Error { get; }

		/// <summary>
		/// Status code of an error
		/// </summary>
		public HttpStatusCode? StatusCode { get; }

		/// <summary>
		/// Creates a successful <see cref="Result{T}"/> containing the specified value.
		/// </summary>
		/// <param name="value">The value produced by a successful operation.</param>
		/// <returns>A <see cref="Result{T}"/> instance with <see cref="IsSuccess"/> = <c>true</c> and <see cref="Value"/> set.</returns>
		public static Result<T> Success(T value)
			=> new(true, value, null, null);

		/// <summary>
		/// Creates a failed <see cref="Result{T}"/> containing the specified error message.
		/// </summary>
		/// <param name="error">A user-friendly error message describing the failure.</param>
		/// <param name="httpStatusCode">Status code of an error</param>
		/// <returns>A <see cref="Result{T}"/> instance with <see cref="IsSuccess"/> = <c>false</c> and <see cref="Error"/> set.</returns>
		public static Result<T> Failure(string error, HttpStatusCode httpStatusCode)
			=> new(false, default, error, httpStatusCode);

		private Result(bool isSuccess, T? value, string? error, HttpStatusCode? httpStatusCode)
		{
			IsSuccess = isSuccess;
			Value = value;
			Error = error;
			StatusCode = httpStatusCode;
		}
	}
}
