using System.Security.Cryptography;

namespace AxiteHR.Services.AuthAPI.Services.Auth.Impl
{
	public class TempPasswordGeneratorService : ITempPasswordGeneratorService
	{
		private static readonly char[] UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
		private static readonly char[] LowercaseChars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
		private static readonly char[] DigitChars = "0123456789".ToCharArray();
		private static readonly char[] SpecialChars = "!@#$%^&*()_-+=<>?".ToCharArray();
		private static readonly char[] AllChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_-+=<>?".ToCharArray();
		private const int PasswordLength = 12;

		public string GenerateTempPassword()
		{
			var passwordChars = new char[PasswordLength];
			var filledPositions = new bool[PasswordLength];

			// Randomly assign required characters
			AssignRandomChar(UppercaseChars, passwordChars, filledPositions);
			AssignRandomChar(LowercaseChars, passwordChars, filledPositions);
			AssignRandomChar(DigitChars, passwordChars, filledPositions);
			AssignRandomChar(SpecialChars, passwordChars, filledPositions);

			// Fill the rest of the password with random characters from all available characters
			for (int i = 0; i < PasswordLength; i++)
			{
				if (!filledPositions[i])
				{
					passwordChars[i] = GetRandomChar(AllChars);
				}
			}

			return new string(passwordChars);
		}

		#region Private Methods
		private static void AssignRandomChar(char[] charSet, char[] passwordChars, bool[] filledPositions)
		{
			int position;
			do
			{
				position = RandomNumberGenerator.GetInt32(passwordChars.Length);
			}
			while (filledPositions[position]);

			passwordChars[position] = GetRandomChar(charSet);
			filledPositions[position] = true;
		}

		private static char GetRandomChar(char[] charSet)
		{
			int randomIndex = RandomNumberGenerator.GetInt32(charSet.Length);
			return charSet[randomIndex];
		}
		#endregion
	}
}
