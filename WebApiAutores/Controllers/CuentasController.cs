using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPIAutores.DTOs;

namespace WebAPIAutores.Controllers
{
	[ApiController]
	[Route("api/cuentas")]
	public class CuentasController : ControllerBase
	{
		private readonly UserManager<IdentityUser> userManager;
		private readonly IConfiguration configuration;
		private readonly SignInManager<IdentityUser> signInManager;

		public CuentasController(UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
			this.userManager = userManager;
			this.configuration = configuration;
			this.signInManager = signInManager;
		}

        [HttpPost("registrar")] // api/cuentas/registrar
		public async Task<ActionResult<RespuestasAutenticacionDTO>> Registrar(CredencialesUsuario credencialesUsuario)
		{
			var usuario = new IdentityUser { UserName = credencialesUsuario.Email, Email = credencialesUsuario.Email };
			var resultado = await userManager.CreateAsync(usuario, credencialesUsuario.Password);

			if (resultado.Succeeded)
			{
				return ConstruirToken(credencialesUsuario);
			}
			else
			{
				return BadRequest(resultado.Errors);
			}
		}
		[HttpPost("login")]
		public async Task<ActionResult<RespuestasAutenticacionDTO>> Login(CredencialesUsuario credencialesUsuario)
		{
			var resultado = await signInManager.PasswordSignInAsync(credencialesUsuario.Email, credencialesUsuario.Password, isPersistent: false, lockoutOnFailure: false);

			if (resultado.Succeeded)
			{
				return ConstruirToken(credencialesUsuario);
			}
			else
			{
				return BadRequest("Login Incorrecto");
			}
		}
		[HttpGet("RenovarToken")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public ActionResult<RespuestasAutenticacionDTO> Renovar()
		{
			var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.Email)
				.FirstOrDefault();
			var email = emailClaim.Value;
			var credencialesUsuario = new CredencialesUsuario() 
			{
				Email = email
			};

			return ConstruirToken(credencialesUsuario);
		}

		private RespuestasAutenticacionDTO ConstruirToken(CredencialesUsuario credencialesUsuario)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Email, credencialesUsuario.Email),		
				new Claim("lo que yo quiera", "Cualquier otro valor")			
			};
			var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));
			var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

			var expiracion = DateTime.UtcNow.AddYears(1);

			var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: credenciales);

			return new RespuestasAutenticacionDTO()
			{
				Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
				Expiracion = expiracion
			};
		}

		[HttpPost("HacerAdmin")]
		public async Task<ActionResult> HacerAdmin(EditarAdminDTO editarAdminDTO)
		{
			var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);
			await userManager.AddClaimAsync(usuario, new Claim("esAdmin", "1"));
			return NoContent();
		}

		[HttpPost("RemoverAdmin")]
		public async Task<ActionResult> RemoverAdmin(EditarAdminDTO editarAdminDTO)
		{
			var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);
			await userManager.RemoveClaimAsync(usuario, new Claim("esAdmin", "1"));
			return NoContent();
		}
	}
}
