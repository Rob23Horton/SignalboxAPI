using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers
{
	public class Hashing
	{
		public static string Hash(string input)
		{
			var inputBytes = Encoding.UTF8.GetBytes(input);
			var inputHash = SHA256.HashData(inputBytes);
			return Convert.ToHexString(inputHash);
		}
	}
}
