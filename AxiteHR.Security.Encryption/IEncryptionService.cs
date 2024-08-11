namespace AxiteHR.Security.Encryption
{
	public interface IEncryptionService
	{
		/// <summary>
		/// This method is used to encrypt text with specific key
		/// </summary>
		/// <param name="plainText"></param>
		/// <param name="key"></param>
		/// <returns>Encrypted text</returns>
		string Encrypt(string plainText, string key);

		/// <summary>
		/// This method is used to decrypt text with specific key
		/// </summary>
		/// <param name="cipherText"></param>
		/// <param name="key"></param>
		/// <returns>Decrypted text</returns>
		string Decrypt(string cipherText, string key);
	}
}
