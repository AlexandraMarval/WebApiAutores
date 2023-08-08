using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Entidades;

namespace WebAPIAutores
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<AutoreLibro>().HasKey(autorlibro => new {autorlibro.AutorId, autorlibro.LibroId});
		}

		public DbSet<Autor> Autores { get; set; }	
		public DbSet<Libro> Libros { get; set; }	
		public DbSet<Comentario> Comentarios { get; set; }	
		public DbSet<AutoreLibro> AutoresLibros { get; set; }
	}
}
