using Microsoft.EntityFrameworkCore;
using ServicoUsuarios.Models;

namespace ServicoUsuarios.Data
{
    public class UsuariosContext : DbContext
    {
        public UsuariosContext(DbContextOptions<UsuariosContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(255);
                entity.Property(e => e.TipoPerfil).IsRequired().HasMaxLength(50);
            });
        }
    }
}

