using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Controllers
{
	[ApiController]
	[Route("Api/libros")]
	public class LibrosController : ControllerBase
	{
		private readonly ApplicationDbContext context;
		private readonly IMapper mapper;

		public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
			this.context = context;
			this.mapper = mapper;
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<LibroDTO>> GetLibro(int id)
		{
			var Libro = await context.Libros.FirstOrDefaultAsync(libro => libro.Id == id);
			return mapper.Map<LibroDTO>(Libro);
		}

		[HttpPost]
		public async Task<ActionResult> PostLibro(LibroCreacionDTO libroCreacionDTO)
		{
			//var existeAutor = await context.Autores.AnyAsync(autor => autor.Id == libro.AutorId);

			//if (!existeAutor)
			//{
			//	return BadRequest($"no existe el autor de Id: {libro.AutorId}");
			//}
			var libro = mapper.Map<Libro>(libroCreacionDTO);
			context.Add(libro);
			await context.SaveChangesAsync();
			return Ok();
		}

		[HttpPut]

		public async Task<ActionResult> PutLibro(Libro libro)
		{			
			context.Update(libro);
			await context.SaveChangesAsync();
			return Ok();
		}
    }
}
