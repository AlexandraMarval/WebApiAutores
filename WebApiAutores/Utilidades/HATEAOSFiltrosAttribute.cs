using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiAutores.Utilidades
{
    public class HATEAOSFiltrosAttribute : ResultFilterAttribute
    {
        protected bool DebeIncluirHATEAOS(ResultExecutingContext Context)
        {
            var resultado = Context.Result as ObjectResult;

            if (!EsRespuestaExitosa(resultado))
            {
                return false;
            }

            var cabecera = Context.HttpContext.Request.Headers["incluirHATEAOS"];

            if (cabecera.Count == 0)
            {
                return false;
            }

            var valor = cabecera[0];

            if (!valor.Equals("y", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            return true;
        }

        private bool EsRespuestaExitosa(ObjectResult result)
        {
            if (result == null || result.Value == null)
            {
                return false;
            }
            if (result.StatusCode.HasValue && !result.StatusCode.Value.ToString().StartsWith("2")) 
            {
                return false;
            }
            return true;
        }
    }
}
