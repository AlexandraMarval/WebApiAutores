using WebAPIAutores.DTOs;

namespace WebApiAutores.DTOs
{
	public class ColeccionDeRecursos<T> : Recursos where T : Recursos
	{
        public List<T>Valores { get; set; }
    }
}
