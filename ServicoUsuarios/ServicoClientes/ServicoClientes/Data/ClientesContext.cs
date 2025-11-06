using Microsoft.EntityFrameworkCore;
using ServicoClientes.Models;

namespace ServicoClientes.Data
{
    public class ClientesContext : DbContext
    {
        public ClientesContext(DbContextOptions<ClientesContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NomeRazaoSocial).IsRequired().HasMaxLength(255);
                entity.Property(e => e.CpfCnpj).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.CpfCnpj).IsUnique();
                entity.Property(e => e.StatusCliente).IsRequired().HasMaxLength(50);
            });
        }
    }
}

