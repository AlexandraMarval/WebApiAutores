using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApiAutores.Utilidades
{
    public class AgregarParametrosHATEAOS : IOperationFilter
    {
        public void Apply(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext)
        {
            if (operationFilterContext.ApiDescription.HttpMethod != "GET")
            {
                return;
            }

            if (openApiOperation == null)
            {
                openApiOperation.Parameters = new List<OpenApiParameter>();
            }

            openApiOperation.Parameters.Add(new OpenApiParameter
            {
                Name = "IncluirHATEAOS",
                In = ParameterLocation.Header,
                Required = false,
            });

        }
    }
}
