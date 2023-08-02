﻿using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Utilidades
{
	public class AutoMapperProfiles : Profile
	{
        public AutoMapperProfiles()
        {
            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<Autor, AutorDTO>();
            CreateMap<LibroCreacionDTO, Libro>().ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));
            CreateMap<Libro, LibroDTO>().ForMember(libroDTO => libroDTO.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores));
            CreateMap<ComentarioCreacionDTO, Comentario>();
            CreateMap<Comentario, ComentarioDTO>();
        }

        private List<AutorDTO> MapLibroDTOAutores(Libro libro, LibroDTO libroDTO)
        {
            var resultado = new List<AutorDTO>();

            if (libro.AutoresLibros == null)
            {
                return resultado;
            }

            foreach (var autorLibro in libro.AutoresLibros )
            {
                resultado.Add(new AutorDTO()
                {
                    Id = autorLibro.AutorId,
                    Nombre = autorLibro.Autor.Nombre
                });
            }

            return resultado;
        }


		private List<AutoreLibro> MapAutoresLibros(LibroCreacionDTO libroCreacionDTO, Libro libro)
        {            
            var resultado = new List<AutoreLibro>();

            if(libroCreacionDTO.AutoresIds == null)
            {
                return resultado;
            }

            foreach (var autorId in libroCreacionDTO.AutoresIds)
            {
                resultado.Add(new AutoreLibro { AutorId = autorId });
            }

            return resultado;


        }
    }
}
