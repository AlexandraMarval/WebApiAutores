using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.DTOs
{
	public class LibroCreacionDTO
	{
		[PrimeraLetraMayuscula]
		[StringLength(maximumLength: 250)]
		public string Titulo { get; set; }
        public List<int> AutoresIds { get; set; }
    }
}
