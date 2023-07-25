using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Controllers
{
	[ApiController]
	[Route("Api/libros")]
	public class LibrosController : ControllerBase
	{
		private readonly ApplicationDbContext context;

		public LibrosController(ApplicationDbContext context)
        {
			this.context = context;
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<Libro>> GetLibro(int id)
		{
			return await context.Libros.Include(libro => libro.Autor).FirstOrDefaultAsync(libro => libro.Id == id);
		}

		[HttpPost]
		public async Task<ActionResult> PostLibro(Libro libro)
		{
			var existeAutor = await context.Autores.AnyAsync(autor => autor.Id == libro.AutorId);

			if (!existeAutor)
			{
				return BadRequest($"no existe el autor de Id: {libro.AutorId}");
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
