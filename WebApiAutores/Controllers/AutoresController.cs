using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Controllers
{
	[ApiController]
	[Route("api/autores")]
	public class AutoresController: ControllerBase
	{
		private readonly ApplicationDbContext context;

		public AutoresController(ApplicationDbContext context)
        {
			this.context = context;
		}
		// Solo podemos utilizar la promagracion Asincrona cuando se quiere hacer peticiones en la webApi en la base datos cuando se encuenta enotro servidor, en la webApi de facebook, Google
        [HttpGet]
		public async Task<ActionResult<List<Autor>>> GetAutor()
		{
			return await context.Autores.Include(x => x.Libros).ToListAsync();
		}

		[HttpGet("primeri")]// Model Binding
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
		public async Task<ActionResult> PostAutor([FromBody] Autor autor)
		{
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
