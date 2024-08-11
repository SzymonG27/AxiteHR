using System.Security.Cryptography;
using System.Text;

namespace AxiteHR.Security.Encryption
{
	public class EncryptionService : IEncryptionService
	{
		public string Encrypt(string plainText, string key)
		{
			using var aesAlg = Aes.Create();
			aesAlg.Key = Encoding.UTF8.GetBytes(key);
			aesAlg.GenerateIV();

			var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

			using var msEncrypt = new MemoryStream();
			msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

			using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
			using (var swEncrypt = new StreamWriter(csEncrypt))
			{
				swEncrypt.Write(plainText);
			}

			var encrypted = msEncrypt.ToArray();
			return Convert.ToBase64String(encrypted);
		}

		public string Decrypt(string cipherText, string key)
		{
			var fullCipher = Convert.FromBase64String(cipherText);

			using var aesAlg = Aes.Create();
			aesAlg.Key = Encoding.UTF8.GetBytes(key);

			var iv = new byte[aesAlg.BlockSize / 8];
			Array.Copy(fullCipher, iv, iv.Length);

			var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, iv);

			using var msDecrypt = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length);
			using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
			using var srDecrypt = new StreamReader(csDecrypt);

			return srDecrypt.ReadToEnd();
		}
	}
}
