using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.Entidades
{
	public class Libro
	{
        public int Id { get; set; }
        [StringLength(maximumLength: 250)]
        public string Titulo { get; set; }
        public int AutorId { get; set; }
        public Autor Autor { get; set; }
    }
}
