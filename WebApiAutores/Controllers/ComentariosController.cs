﻿using AutoMapper;
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
		public async Task<ActionResult<List<ComentarioDTO>>> GetComentarios(int libroId)
		{
			var existeLibro = await context.Libros.AnyAsync(libro => libroId == libroId);

			if (!existeLibro)
			{
				return NotFound("");
			}
			var comentarios = await context.Comentarios.Where(comentario => comentario.LibroId == libroId).ToListAsync();
			return mapper.Map<List<ComentarioDTO>>(comentarios);
		}

		[HttpGet("id:int", Name = "ObtenerComentario")]
		public async Task<ActionResult<ComentarioDTO>> GetComentariosPorId(int id)
		{
			var comentario = await context.Comentarios.FirstOrDefaultAsync(comentario => comentario.Id == id);

			if (comentario == null) 
			{
				return NotFound();
			}

			return mapper.Map<ComentarioDTO>(comentario);
		}


		[HttpPost]
		public async Task<ActionResult> PostComentario(int libroId, ComentarioCreacionDTO comentarioCreacionDTO)
		{
			var existeLibro = await context.Libros.AnyAsync(libro => libro.Id == libroId);

			if(!existeLibro) 
			{
				return NotFound("");
			}

			var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
			comentario.LibroId = libroId;

			context.Add(comentario);
			await context.SaveChangesAsync();

			var comentarioDTO = mapper.Map<ComentarioDTO>(comentario);
			return CreatedAtRoute("ObtenerComentario", new { id = comentario.Id, libroId = libroId}, comentarioDTO);
		}

		[HttpPut("id:int")]
		public async Task<ActionResult> PutComentario(int libroId, int id, ComentarioCreacionDTO comentarioCreacionDTO)
		{
			var existeLibro = await context.Libros.AnyAsync(libro => libro.Id == libroId);

			if(!existeLibro)
			{
				return NotFound();
			}

			var existeComentario = await context.Comentarios.AnyAsync(comentario => comentario.Id == id);

			if (!existeComentario)
			{
				return NotFound();
			}

			var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
			comentario.Id = id;
			comentario.LibroId = libroId;
			context.Update(comentario);
			await context.SaveChangesAsync();
			return NoContent();
			
		}
	}
}
