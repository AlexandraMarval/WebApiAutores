using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIAutores.DTOs;

namespace WebAPIAutores.Controllers
{
	[ApiController]
	[Route("api")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class RootController : ControllerBase
	{
		private readonly IAuthorizationService authorizationService;

		public RootController(IAuthorizationService authorizationService)
        {
			this.authorizationService = authorizationService;
		}

        [HttpGet(Name = "ObtenerRoot")]
		[AllowAnonymous]
		public async Task<ActionResult<IEnumerable<DatosHATEOAS>>> Get()
		{
		try
		{
				var datosHateaos = new List<DatosHATEOAS>();
	
				var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin", "1");
	
				datosHateaos.Add(new DatosHATEOAS(enlace: Url.Link("ObtenerRoot", new { }), descripcion: "self", metodo: "GET"));
	
				datosHateaos.Add(new DatosHATEOAS(enlace: Url.Link("obtenerAutores", new { }), descripcion: "autores", metodo: "GET"));
	
				if (esAdmin.Succeeded)
				{
					datosHateaos.Add(new DatosHATEOAS(enlace: Url.Link("crearAutor", new { }), descripcion: "autor-crear", metodo: "POST"));
	
					datosHateaos.Add(new DatosHATEOAS(enlace: Url.Link("crearLibro", new { }), descripcion: "libro-crear", metodo: "POST"));
				}
				return datosHateaos;
		}
		catch (System.Exception ex)
		{
			
			throw ex;
		}
		}
	}
}
