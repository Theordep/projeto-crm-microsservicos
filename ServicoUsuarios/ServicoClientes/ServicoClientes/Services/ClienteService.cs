using Microsoft.EntityFrameworkCore;
using ServicoClientes.Data;
using ServicoClientes.DTOs;
using ServicoClientes.Models;

namespace ServicoClientes.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ClientesContext _context;

        public ClienteService(ClientesContext context)
        {
            _context = context;
        }

        public async Task<ClienteDTO?> CriarClienteAsync(CriarClienteDTO dto)
        {
            // Verificar se CPF/CNPJ já existe
            if (await _context.Clientes.AnyAsync(c => c.CpfCnpj == dto.CpfCnpj))
            {
                throw new Exception("CPF/CNPJ já cadastrado");
            }

            var cliente = new Cliente
            {
                NomeRazaoSocial = dto.NomeRazaoSocial,
                CpfCnpj = dto.CpfCnpj,
                RepresentanteId = dto.RepresentanteId,
                StatusCliente = "Prospect"
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return new ClienteDTO
            {
                Id = cliente.Id,
                NomeRazaoSocial = cliente.NomeRazaoSocial,
                CpfCnpj = cliente.CpfCnpj,
                RepresentanteId = cliente.RepresentanteId,
                StatusCliente = cliente.StatusCliente
            };
        }

        public async Task<ClienteDTO?> ObterClientePorIdAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return null;

            return new ClienteDTO
            {
                Id = cliente.Id,
                NomeRazaoSocial = cliente.NomeRazaoSocial,
                CpfCnpj = cliente.CpfCnpj,
                RepresentanteId = cliente.RepresentanteId,
                StatusCliente = cliente.StatusCliente
            };
        }

        public async Task<List<ClienteDTO>> ListarClientesAsync()
        {
            return await _context.Clientes
                .Select(c => new ClienteDTO
                {
                    Id = c.Id,
                    NomeRazaoSocial = c.NomeRazaoSocial,
                    CpfCnpj = c.CpfCnpj,
                    RepresentanteId = c.RepresentanteId,
                    StatusCliente = c.StatusCliente
                })
                .ToListAsync();
        }

        public async Task<ClienteDTO?> AtualizarStatusAsync(int id, AtualizarStatusClienteDTO dto)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return null;

            cliente.StatusCliente = dto.StatusCliente;
            await _context.SaveChangesAsync();

            return new ClienteDTO
            {
                Id = cliente.Id,
                NomeRazaoSocial = cliente.NomeRazaoSocial,
                CpfCnpj = cliente.CpfCnpj,
                RepresentanteId = cliente.RepresentanteId,
                StatusCliente = cliente.StatusCliente
            };
        }
    }
}

