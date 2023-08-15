namespace WebAPIAutores.DTOs
{
	public class RespuestasAutenticacionDTO
	{
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
    }
}
