using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApiAutores.Utilidades
{
    public class AgregarParametroXVersion : IOperationFilter
    {
        public void Apply(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext)
        {
            if (openApiOperation == null)
            {
                openApiOperation.Parameters = new List<OpenApiParameter>();
            }

            openApiOperation.Parameters.Add(new OpenApiParameter
            {
                Name = "x-version",
                In = ParameterLocation.Header,
                Required = true,
            });

        }
    }
}
