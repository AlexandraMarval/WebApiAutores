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
			var Libro = await context.Libros.Include(libro => libro.AutoresLibros).ThenInclude(autorLibro => autorLibro.Autor).FirstOrDefaultAsync(libro => libro.Id == id);
			Libro.AutoresLibros = Libro.AutoresLibros.OrderBy(libro => libro.Orden).ToList();
			return mapper.Map<LibroDTO>(Libro);
		}

		[HttpPost]
		public async Task<ActionResult> PostLibro(LibroCreacionDTO libroCreacionDTO)
		{
			if (libroCreacionDTO.AutoresIds == null)
			{
				return BadRequest("No se puede crear un libro sin autores");
			}

			var autoresIds = await context.Autores.Where(autores => libroCreacionDTO.AutoresIds.Contains(autores.Id)).Select(autor => autor.Id).ToListAsync();

			if (libroCreacionDTO.AutoresIds.Count != autoresIds.Count)
			{
				return BadRequest("No existe uno de los autores enviados");
			}
			
			var libro = mapper.Map<Libro>(libroCreacionDTO);

			if (libro.AutoresLibros != null )
			{
				for (int i = 0; i < libro.AutoresLibros.Count; i++)
				{
					libro.AutoresLibros[i].Orden = i;
				}
			}
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
