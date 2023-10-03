using Microsoft.EntityFrameworkCore;

namespace WebApiAutores.Utilidades
{
    public static class HttpContextExtensions
    {
        public static async Task InsertarParametrosPaginacionEnCabecera<T>(this HttpContext httpcontext, IQueryable<T> values) 
        {
            if (httpcontext == null) { throw new ArgumentNullException(nameof(httpcontext)); }

            double cantidad = await values.CountAsync();
            httpcontext.Response.Headers.Add("cantidadTotalRegistros", cantidad.ToString());
        }
    }
}
