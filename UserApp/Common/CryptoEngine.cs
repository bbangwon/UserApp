using System.Security.Cryptography;
using System.Text;

namespace UserApp.Common
{
	public class CryptoEngine
	{
		public static string EncryptPassword(string password)
		{
			return SHA256Hash(MD5Hash(password));
		}

		public static string MD5Hash(string input)
		{
			using var md5 = MD5.Create();
			var result = md5.ComputeHash(Encoding.Default.GetBytes(input));
			return Encoding.Default.GetString(result);
		}

		public static string SHA256Hash(string text)
		{
			using var sha256 = SHA256.Create();
			var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
			return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
		}
	}
}
