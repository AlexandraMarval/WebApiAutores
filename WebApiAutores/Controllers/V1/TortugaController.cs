
using Microsoft.AspNetCore.Mvc;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebApiAutores.Controllers.V1;

[ApiController]
[Route("api/v1/tortuga")]
public class TortugaController : ControllerBase
{
    [HttpGet]
    public IActionResult Get(string texto)
    {
        var tortuga = new TortugaGrande();
        tortuga.Hablar(texto);
        return Ok(tortuga);
    }

    [HttpGet("GiraTortuguita")]
    public IActionResult GiraTortuga(int numerosDeVueltas)
    {
        var tortuga = new TortugaGrande();
        tortuga.DarVueltas(numerosDeVueltas);
        return Ok(tortuga);
    }


    [HttpGet("Gira")]
    public IActionResult Gira()
    {
        var tortuga = new TortugaGrande();
        tortuga.CantidadVueltas = 90;
        return Ok(tortuga);
    }
}
