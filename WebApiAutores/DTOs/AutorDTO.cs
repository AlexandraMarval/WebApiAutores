using System.ComponentModel.DataAnnotations;

namespace WebAPIAutores.DTOs
{
	public class AutorDTO : Recursos
	{
        public int Id { get; set; }
		[Required(ErrorMessage = "El campo {0} es requerido")]
		[StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe de tener mas de {1} carácter")]
		public string Nombre { get; set; }    
    }
}
