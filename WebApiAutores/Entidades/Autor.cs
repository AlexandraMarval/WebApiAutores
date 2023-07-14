using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.Entidades
{
	public class Autor
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "El campo {0} es requerido")]
		[StringLength(maximumLength: 10, ErrorMessage = "El campo {0} no debe de tener mas de {1} carácter")]
		// Personalizar atributos
		[PrimeraLetraMayuscula]
        public string Nombre { get; set; }
		[Range(18, 68)]
		[NotMapped]
        public int Edad { get; set; }
		[CreditCard]
		[NotMapped]
		public string TarjetaDeCredito { get; set; }
		[Url]
		[NotMapped]
		public string Url  { get; set; }
        public List<Libro> Libros { get; set; }
	}
}
