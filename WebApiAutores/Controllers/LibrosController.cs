using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
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

		[HttpGet("{id:int}", Name = "obtenerLibros")]
		public async Task<ActionResult<LibroDTOConAutores>> GetLibro(int id)
		{
			var libro = await context.Libros.Include(libro => libro.AutoresLibros).ThenInclude(autorLibro => autorLibro.Autor).FirstOrDefaultAsync(libro => libro.Id == id);

			if (libro == null)
			{
				return NotFound();
			}
			libro.AutoresLibros = libro.AutoresLibros.OrderBy(libro => libro.Orden).ToList();
			return mapper.Map<LibroDTOConAutores>(libro);
		}

		[HttpPost(Name = "crearLibro")]
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
			AsignarOrdenAutores(libro);
			
			context.Add(libro);
			await context.SaveChangesAsync();
			return Ok();
		}

		[HttpPut(Name = "actualizarLibro")] 
		public async Task<ActionResult> PutLibro(int id, LibroCreacionDTO libroCreacionDTO)
		{
			var libro = await context.Libros.Include(libro => libro.AutoresLibros).FirstOrDefaultAsync(libro => libro.Id == id);

			if (libro == null)
			{
				return NotFound();
			}

			libro = mapper.Map(libroCreacionDTO, libro);
			AsignarOrdenAutores(libro);
			
			await context.SaveChangesAsync();
			return NoContent();
		}

		private void AsignarOrdenAutores(Libro libro)
		{
			if (libro.AutoresLibros != null)
			{
				for (int i = 0; i < libro.AutoresLibros.Count; i++)
				{
					libro.AutoresLibros[i].Orden = i;
				}
			}
		}

		[HttpPatch("id:int", Name = "patchLibro")]
		public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> patchDocument)
		{
			if (patchDocument == null)
			{
				return BadRequest();
			}

			var libro = await context.Libros.FirstOrDefaultAsync(libro
				 => libro.Id == id);

			if (libro == null)
			{
				return NotFound();
			}

			var libroDTO = mapper.Map<LibroPatchDTO>(libro);

			patchDocument.ApplyTo(libroDTO, ModelState);

			var esValido = TryValidateModel(libro);

			if (!esValido)
			{
				return BadRequest();
			}
			mapper.Map(libroDTO, libro);

			await context.SaveChangesAsync();
			return NoContent();
		}

		[HttpDelete("{id:int}", Name = "borrarLibro")]
		public async Task<ActionResult> DeleteLibro(int id)
		{
			var existe = await context.Libros.AnyAsync(libro => libro.Id == id);
			if (!existe)
			{
				return NotFound();
			}
			context.Remove(new Libro() { Id = id });
			await context.SaveChangesAsync();
			return NoContent();
		}
	}
}
