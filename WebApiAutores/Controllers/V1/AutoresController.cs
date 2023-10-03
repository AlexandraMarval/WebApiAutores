using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Utilidades;
using WebAPIAutores;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;
using WebAPIAutores.Filtros;
using WebAPIAutores.Servicios;
using static WebAPIAutores.Servicios.ServicioB;

namespace WebApiAutores.Controllers.V1
{
    [ApiController]
    [Route("api/autores")]
    [CabeceraEstaPresente("x-version", "1")]
    //[Route("api/v1/autores")]
    ////[Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IService service;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioSingleton servicioSingleton;
        private readonly ServicioScoped servicioScoped;
        private readonly ILogger<AutoresController> logger;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IAuthorizationService authorizationService;

        public AutoresController(ApplicationDbContext context, IService service, ServicioTransient servicioTransient, ServicioSingleton servicioSingleton, ServicioScoped servicioScoped, ILogger<AutoresController> logger, IMapper mapper, IConfiguration configuration, IAuthorizationService authorizationService)
        {
            this.context = context;
            this.service = service;
            this.servicioTransient = servicioTransient;
            this.servicioSingleton = servicioSingleton;
            this.servicioScoped = servicioScoped;
            this.logger = logger;
            this.mapper = mapper;
            this.configuration = configuration;
            this.authorizationService = authorizationService;
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
        //[HttpGet("int:id")]
        //public async Task<ActionResult<AutorDTO>> Get(int id)
        //{
        //	var autor = await context.Autores.FirstAsync(autor => autor.Id == id);
        //	var respuesta = new AutorDTO
        //	{
        //		Id = autor.Id,
        //		Nombre = autor.Nombre
        //	};
        //	return respuesta;			
        //}

        //[HttpGet("configuracionesV1")]
        //public ActionResult<string> ObtenerConfiguracion()
        //{
        //	return configuration["nombre"];
        //}

        /// <summary>
        /// listado de autores
        /// </summary>
        /// <returns></returns>
        [HttpGet("listado", Name = "obtenerListadoDeAutoresV1")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEAOSAutorFilterAttribute))]
        //[Authorize]
        //[ResponseCache(Duration = 10)]
        //[ServiceFilter(typeof(MiFiltroDeAccion))]
        public async Task<ActionResult<List<AutorDTO>>> GetAutor([FromQuery] PaginacionDTO paginacionDTO)
        {
            //	logger.LogInformation("Estamos obteniendo los autores");
            //	logger.LogWarning("Este es un mensaje de prueba");
            var autores = await context.Autores.ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);
        }

        //[HttpGet(Name = "obtenerAutoresV1")]// Model Binding/
        //[AllowAnonymous]
        //      public async Task<ActionResult<List<Autor>>> PrimerAutor()
        //{
        //	var autores = await context.Autores.ToListAsync();
        //	return mapper.Map<List<Autor>>(autores);
        //}
        // podemos poner rutas con restricciones o podemos poner ruta opcionales o lo que queramos
        // El NotFound hereda de ActionResult/ La diferencia de ActionResult de T a IActionResult es que con IActionResult no puedo retornar  un autor 

        /// <summary>
        /// obtener autor por id
        /// </summary>
        /// <param name="id">Obtener un autor</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "obtenerAutorV1")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEAOSAutorFilterAttribute))]
        public async Task<ActionResult<AutorDTOConLibros>> GetPrimerAutor(int id)
        {
            var autor = await context.Autores
                .Include(autor => autor.AutoresLibros)
                .ThenInclude(autorLibro => autorLibro.Libro)
                .FirstOrDefaultAsync(autor => autor.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            var DTO = mapper.Map<AutorDTOConLibros>(autor);
            return DTO;
        }

        /// <summary>
        /// obtener autor por nombre
        /// </summary>
        /// <param name="nombre">Obtener autor por nombre</param>
        /// <returns></returns>
        [HttpGet("por-nombre", Name = "obtenerAutoresPorNombreV1")]
        [AllowAnonymous] // Se utiliza para las personas que no necesitan autenticarse
        public async Task<ActionResult<List<AutorDTO>>> GetNombreAutor(string nombre)
        {
            var autores = await context.Autores.Where(autor => autor.Nombre.Contains(nombre)).ToListAsync();

            return mapper.Map<List<AutorDTO>>(autores);
        }
        /// <summary>
        /// crear autor
        /// </summary>
        /// <param name="autorCreacionDTO">Crear un autor</param>
        /// <returns></returns>
        [HttpPost(Name = "crearAutorV1")]
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

            return CreatedAtRoute("obtenerAutorIdV1", new { id = autor.Id }, autorDTO);
        }
        /// <summary>
        /// actualizar un autor
        /// </summary>
        /// <param name="autorCreacionDTO">Actualizar autor por id</param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id:int}", Name = "actualizarAutorV1")]
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

        /// <summary>
        /// Borra un autor
        /// </summary>
        /// <param name="id">Id del autor a borrar</param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "eliminarAutorV1")]
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
