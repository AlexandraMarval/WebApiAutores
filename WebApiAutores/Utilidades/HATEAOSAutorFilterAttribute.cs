using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiAutores.Servicios;
using WebAPIAutores.DTOs;

namespace WebApiAutores.Utilidades
{
    public class HATEAOSAutorFilterAttribute : HATEAOSFiltrosAttribute
    {
        private readonly GeneradorEnlaces enlaces;

        public HATEAOSAutorFilterAttribute(GeneradorEnlaces enlaces)
        {
            this.enlaces = enlaces;
        }
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate executionDelegate) 
        {
            var debeIncluir = DebeIncluirHATEAOS(context);

            if (!debeIncluir) 
            {
                await executionDelegate();
                return;
            }

            var resultado = context.Result as ObjectResult;
            var autorDTO = resultado.Value as AutorDTO;
            if(autorDTO == null) 
            {
                var autoresDTO = resultado.Value as List<AutorDTO> ?? throw new ArgumentException("Se esperaba una instancia de AutorDTO o List<AutorDTO>");
                autoresDTO.ForEach(async autor => await enlaces.GenerarEnlaces(autor));
                resultado.Value = autoresDTO;
            }
            else
            {
                await enlaces.GenerarEnlaces(autorDTO);
            }
            await executionDelegate();
        }
    }
}
