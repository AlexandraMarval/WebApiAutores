using System.Security.Cryptography;
using WebAPIAutores.DTOs;

namespace WebAPIAutores.Servicios
{
	public class HashService
	{
		public ResultadoHash Hash(string textoPlano)
		{
			var sal = new byte[16];
			using (var random = RandomNumberGenerator.Create())
			{
				random.GetBytes(sal);
			}
			
		}
	}
}
