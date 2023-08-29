using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;
using WebAPIAutores.Filtros;
using WebAPIAutores.Servicios;
using static WebAPIAutores.Servicios.ServicioB;

namespace WebAPIAutores.Controllers
{
    [ApiController]
	[Route("api/autores")]
	//[Authorize]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
	public class AutoresController: ControllerBase
	{
		private readonly ApplicationDbContext context;
		private readonly IService service;
		private readonly ServicioTransient servicioTransient;
		private readonly ServicioSingleton servicioSingleton;
		private readonly ServicioScoped servicioScoped;
		private readonly ILogger<AutoresController> logger;
		private readonly IMapper mapper;
		private readonly IConfiguration configuration;

		public AutoresController(ApplicationDbContext context, IService service, ServicioTransient servicioTransient, ServicioSingleton servicioSingleton, ServicioScoped servicioScoped, ILogger<AutoresController> logger, IMapper mapper, IConfiguration configuration )
        {
			this.context = context;
			this.service = service;
			this.servicioTransient = servicioTransient;
			this.servicioSingleton = servicioSingleton;
			this.servicioScoped = servicioScoped;
			this.logger = logger;
			this.mapper = mapper;
			this.configuration = configuration;
		}
		// Solo podemos utilizar la promagracion Asincrona cuando se quiere hacer peticiones en la webApi en la base datos cuando se encuenta en otro servidor, en la webApi de facebook, Google
		//[HttpGet("GUID")]
		// Este ResponseCache nos permite que la pedicion que el usuario haga se mantenga durante 10minutos es un Filtro 
		//[ResponseCache(Duration = 10)]
		//[ServiceFilter(typeof(MiFiltroDeAccion))]
		//public ActionResult ObtenerGuids()
		//{
		//	return Ok(new
		//	{
		//		AutoresControllerTramsient = servicioTransient.Guid,
		//		ServicioA_Transient = service.ObtenerTransient(),
		//		AutoresControllerScoped = servicioScoped.Guid,
		//		ServicioA_Scoped = service.ObtenerScoped(),
		//		AutoresControllerSingleton = servicioSingleton.Guid,
		//		ServicioA_Singleton = service.ObtenerSingleton(),
		//	});
		//} 
		[HttpGet("int:id")]
		public async Task<ActionResult<AutorDTO>> Get(int id)
		{
			var autor = await context.Autores.FirstAsync(autor => autor.Id == id);
			var respuesta = new AutorDTO
			{
				Id = autor.Id,
				Nombre = autor.Nombre
			};
			return respuesta;			
		}

		[HttpGet("configuraciones")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public ActionResult<string> ObtenerConfiguracion()
		{
			return configuration["nombre"];
		}

		[HttpGet("listado", Name = "obtenerListadoDeAutores")]		
		//[Authorize]
		//[ResponseCache(Duration = 10)]
		//[ServiceFilter(typeof(MiFiltroDeAccion))]
		public async Task<ActionResult<List<AutorDTO>>> GetAutor()
		{			
		//	logger.LogInformation("Estamos obteniendo los autores");
		//	logger.LogWarning("Este es un mensaje de prueba");
			var autores = await context.Autores.ToListAsync();
			return mapper.Map<List<AutorDTO>>(autores);
		}

		[HttpGet(Name = "obtenerAutores")]// Model Binding/
		[AllowAnonymous]
		public async Task<ActionResult<List<Autor>>> PrimerAutor([FromHeader] int miValor )
		{
			var autores = await context.Autores.ToListAsync();
			return mapper.Map<List<Autor>>(autores);
		}
		// podemos poner rutas con restricciones o podemos poner ruta opcionales o lo que queramos
		// El NotFound hereda de ActionResult/ La diferencia de ActionResult de T a IActionResult es que con IActionResult no puedo retornar  un autor 

		[HttpGet("{id:int}", Name ="obtenerAutorId")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<ActionResult<AutorDTOConLibros>> GetPrimerAutor(int id)
		{																
			var autor = await context.Autores
				.Include(autor => autor.AutoresLibros)
				.ThenInclude(autorLibro  => autorLibro.Libro)
				.FirstOrDefaultAsync(autor => autor.Id == id);

			if (autor == null)
			{
				return NotFound();
			}

			return mapper.Map<AutorDTOConLibros>(autor);	
		}

		[HttpGet("por-nombre", Name = "obtenerAutoresPorNombre")]
		[AllowAnonymous] // Se utiliza para las personas que no necesitan autenticarse
		public async Task<ActionResult<List<AutorDTO>>> GetNombreAutor(string nombre)
		{
			var autores = await context.Autores.Where(autor => autor.Nombre.Contains(nombre)).ToListAsync();

			return mapper.Map<List<AutorDTO>>(autores);
		}

		[HttpPost(Name = "crearAutor")]
		public async Task<ActionResult> PostAutor([FromBody] AutorCreacionDTO autorCreacionDTO)
		{
			var existeAutorConElMismoNombre = await context.Autores.AnyAsync(autor => autor.Nombre == autorCreacionDTO.Nombre);

			if (existeAutorConElMismoNombre)
			{
				return BadRequest($"Ya existe un autor con el mismo nombre {autorCreacionDTO.Nombre}");
			}

			var autor = mapper.Map<Autor>(autorCreacionDTO);			

			context.Add(autor);
			await context.SaveChangesAsync();

			var autorDTO = mapper.Map<AutorDTO>(autor);

			return CreatedAtRoute("obtenerAutorId", new { id = autor.Id}, autorDTO);
		}

		[HttpPut("{id:int}", Name = "actualizarAutor")]
		public async Task<ActionResult> PutActualizarActores(AutorCreacionDTO autorCreacionDTO, int id)
		{		
			var existe = await context.Autores.AnyAsync(autor => autor.Id == id);

			if (!existe)
			{
				return NotFound();
			}
			var autor = mapper.Map<Autor>(autorCreacionDTO);
			autor.Id = id;

			context.Update(autor);
			await context.SaveChangesAsync();
			return NoContent();
		}

		[HttpDelete("{id:int}", Name = "eliminarAutor")]
		public async Task<ActionResult> DeleteAutor(int id)
		{
			var existe = await context.Autores.AnyAsync(autor => autor.Id == id);
			if (!existe)
			{
				return NotFound();
			}
			context.Remove(new Autor() { Id = id });
			await context.SaveChangesAsync();
			return NoContent();
		}

	}
}
