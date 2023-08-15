using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
using WebAPIAutores.Filtros;
using WebAPIAutores.Middleware;
using WebAPIAutores.Servicios;
using static WebAPIAutores.Servicios.ServicioB;

namespace WebAPIAutores
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{

			services.AddControllers(opciones =>
			{
				opciones.Filters.Add(typeof(FiltroDeExcepcion));
			}).AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).AddNewtonsoftJson();

			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			// transient sirve cuando te va a dar una nueva instancia 
			// Scoped es cuando te va a dar la misma instancia en el mismo contesto HTTP
			//singlenton te va a dar la misma instancia en distintas pediciones HTTP que sean usuario distintos
			services.AddTransient<IService, ServicioA>();

			services.AddSingleton<ServicioSingleton>();
			services.AddTransient<ServicioTransient>();
			services.AddScoped<ServicioScoped>();
			services.AddTransient<MiFiltroDeAccion>();
			//services.AddHostedService<EscribirEnArchivo>();

			services.AddResponseCaching();
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opciones => opciones.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = false,
				ValidateAudience = false,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["llavejwt"])),
				ClockSkew = TimeSpan.Zero
			});

			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiAutores", Version = "v1" });
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "jwt",
					In = ParameterLocation.Header
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						new string[]{}
					}
				});
			});

			services.AddAutoMapper(typeof(Startup));

			services.AddIdentity<IdentityUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.AddAuthorization(opciones =>
			{
				opciones.AddPolicy("EsAdmin", politica => politica.RequireClaim("esAdmin"));
			});
			
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
		{
			// Los Middleware Confugura si un usuario haceuna pedicion y yo quiero me mi aplicacion le muestre un mensaje
			// Este codigo sirve para cualquier controlador

			//app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();
			app.UseLoguearRespuestaHTTP();

			app.Map("/ruta1", app =>
			{
				app.Run(async contexto =>
				{
					await contexto.Response.WriteAsync("Estoy interceptando la tuberia");
				});
			});			
			// Configure the HTTP request pipeline.
			if (env.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseResponseCaching();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
