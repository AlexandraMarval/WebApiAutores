using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Controllers
{
	[ApiController]
	[Route("Api/libros/{libroId:int}/Comentarios")]
	public class ComentariosController : ControllerBase
	{
		private readonly ApplicationDbContext context;
		private readonly IMapper mapper;

		public ComentariosController(ApplicationDbContext context, IMapper mapper) 
		{
			this.context = context;
			this.mapper = mapper;
		}
		[HttpGet]
		public async Task<ActionResult> GetComentarios()
		{


		}
		[HttpPost]
		public async Task<ActionResult> PostComentario(int libroId, ComentarioCreacionDTO comentarioCreacionDTO)
		{
			var existeLibro = await context.Libros.AnyAsync(libro => libroId == libroId);

			if(!existeLibro) 
			{
				return NotFound("");
			}
			var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
			comentario.LibroId = libroId;

			context.Add(comentario);
			await context.SaveChangesAsync();
			return Ok();
		}
	}
}
