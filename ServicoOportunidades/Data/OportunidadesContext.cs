using Microsoft.EntityFrameworkCore;
using ServicoOportunidades.Models;

namespace ServicoOportunidades.Data
{
    public class OportunidadesContext : DbContext
    {
        public OportunidadesContext(DbContextOptions<OportunidadesContext> options) : base(options)
        {
        }

        public DbSet<Ficha> Fichas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ficha>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TituloObra).IsRequired().HasMaxLength(255);
                entity.Property(e => e.StatusFicha).IsRequired().HasMaxLength(50);
            });
        }
    }
}

