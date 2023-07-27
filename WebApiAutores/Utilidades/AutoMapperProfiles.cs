using AutoMapper;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Utilidades
{
	public class AutoMapperProfiles : Profile
	{
        public AutoMapperProfiles()
        {
            CreateMap<AutorCreacionDTO, Autor>();
        }
    }
}
