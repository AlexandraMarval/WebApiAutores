using AutoMapper;
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
	public class AutoresController: ControllerBase
	{
		private readonly ApplicationDbContext context;
		private readonly IService service;
		private readonly ServicioTransient servicioTransient;
		private readonly ServicioSingleton servicioSingleton;
		private readonly ServicioScoped servicioScoped;
		private readonly ILogger<AutoresController> logger;
		private readonly IMapper mapper;

		public AutoresController(ApplicationDbContext context, IService service, ServicioTransient servicioTransient, ServicioSingleton servicioSingleton, ServicioScoped servicioScoped, ILogger<AutoresController> logger, IMapper mapper)
        {
			this.context = context;
			this.service = service;
			this.servicioTransient = servicioTransient;
			this.servicioSingleton = servicioSingleton;
			this.servicioScoped = servicioScoped;
			this.logger = logger;
			this.mapper = mapper;
		}
		// Solo podemos utilizar la promagracion Asincrona cuando se quiere hacer peticiones en la webApi en la base datos cuando se encuenta en otro servidor, en la webApi de facebook, Google
		[HttpGet("GUID")]
		// Este ResponseCache nos permite que la pedicion que el usuario haga se mantenga durante 10minutos es un Filtro 
		//[ResponseCache(Duration = 10)]
		//[ServiceFilter(typeof(MiFiltroDeAccion))]
		public ActionResult ObtenerGuids()
		{
			return Ok(new
			{
				AutoresControllerTramsient = servicioTransient.Guid,
				ServicioA_Transient = service.ObtenerTransient(),
				AutoresControllerScoped = servicioScoped.Guid,
				ServicioA_Scoped = service.ObtenerScoped(),
				AutoresControllerSingleton = servicioSingleton.Guid,
				ServicioA_Singleton = service.ObtenerSingleton(),
			});
		}

        [HttpGet("listado")]
		//[ResponseCache(Duration = 10)]
		//[ServiceFilter(typeof(MiFiltroDeAccion))]
		public async Task<ActionResult<List<Autor>>> GetAutor()
		{			
			logger.LogInformation("Estamos obteniendo los autores");
			logger.LogWarning("Este es un mensaje de prueba");
			return await context.Autores.Include(x => x.Libros).ToListAsync();
		}

		[HttpGet("primero")]// Model Binding
		public async Task<ActionResult<Autor>> PrimerAutor([FromHeader] int miValor )
		{
			return await context.Autores.FirstOrDefaultAsync();
		}
		// podemos poner rutas con restricciones o podemos poner ruta opcionales o lo que queramos
		// El NotFound hereda de ActionResult/ La diferencia de ActionResult de T a IActionResult es que con IActionResult no puedo retornar  un autor 
		[HttpGet("{id:int}")]
		public async Task<ActionResult<Autor>> GetPrimerAutor(int id)
		{																
			var autor = await context.Autores.FirstOrDefaultAsync(autor => autor.Id == id);

			if (autor == null)
			{
				return NotFound();
			}

			return autor;
		}

		[HttpPost]
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
			return Ok();
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult> PutActualizarActores(Autor autor, int id)
		{
			if(autor.Id != id)
			{
				return BadRequest("El id del autor no coincide con el id de la URL");
			}

			var existe = await context.Autores.AnyAsync(autor => autor.Id == id);

			if (!existe)
			{
				return NotFound();
			}
			context.Update(autor);
			await context.SaveChangesAsync();
			return Ok();
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeleteAutor(int id)
		{
			var existe = await context.Autores.AnyAsync(autor => autor.Id == id);
			if (!existe)
			{
				return NotFound();
			}
			context.Remove(new Autor() { Id = id });
			await context.SaveChangesAsync();
			return Ok();
		}

	}
}
