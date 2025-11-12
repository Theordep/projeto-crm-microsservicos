using Microsoft.EntityFrameworkCore;
using ServicoClientes.Data;
using ServicoClientes.DTOs;
using ServicoClientes.Models;

namespace ServicoClientes.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ClientesContext _context;
        private readonly IntegracaoService _integracaoService;

        public ClienteService(ClientesContext context, IntegracaoService integracaoService)
        {
            _context = context;
            _integracaoService = integracaoService;
        }

        public async Task<ClienteDTO?> CriarClienteAsync(CriarClienteDTO dto)
        {
            // Consulta 1: Validar se representante existe no ServicoUsuarios
            var representante = await _integracaoService.ObterUsuarioAsync(dto.RepresentanteId);
            if (representante == null)
            {
                throw new Exception("Representante não encontrado");
            }

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
                NomeRepresentante = representante.Nome,
                StatusCliente = cliente.StatusCliente
            };
        }

        public async Task<ClienteDTO?> ObterClientePorIdAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return null;

            // Consulta 2: Buscar dados do representante no ServicoUsuarios
            var representante = await _integracaoService.ObterUsuarioAsync(cliente.RepresentanteId);

            return new ClienteDTO
            {
                Id = cliente.Id,
                NomeRazaoSocial = cliente.NomeRazaoSocial,
                CpfCnpj = cliente.CpfCnpj,
                RepresentanteId = cliente.RepresentanteId,
                NomeRepresentante = representante?.Nome,
                StatusCliente = cliente.StatusCliente
            };
        }

        public async Task<List<ClienteDTO>> ListarClientesAsync()
        {
            var clientes = await _context.Clientes.ToListAsync();
            var clientesDTO = new List<ClienteDTO>();

            foreach (var cliente in clientes)
            {
                // Consulta 2: Buscar dados do representante no ServicoUsuarios
                var representante = await _integracaoService.ObterUsuarioAsync(cliente.RepresentanteId);

                clientesDTO.Add(new ClienteDTO
                {
                    Id = cliente.Id,
                    NomeRazaoSocial = cliente.NomeRazaoSocial,
                    CpfCnpj = cliente.CpfCnpj,
                    RepresentanteId = cliente.RepresentanteId,
                    NomeRepresentante = representante?.Nome,
                    StatusCliente = cliente.StatusCliente
                });
            }

            return clientesDTO;
        }

        public async Task<ClienteDTO?> AtualizarStatusAsync(int id, AtualizarStatusClienteDTO dto)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return null;

            var statusAnterior = cliente.StatusCliente;
            cliente.StatusCliente = dto.StatusCliente;
            await _context.SaveChangesAsync();

            // Alteração: Se status mudou para "Cancelado", cancelar fichas relacionadas no ServicoOportunidades
            if (statusAnterior != "Cancelado" && dto.StatusCliente == "Cancelado")
            {
                await _integracaoService.CancelarFichasPorClienteAsync(cliente.Id);
            }

            // Buscar dados do representante
            var representante = await _integracaoService.ObterUsuarioAsync(cliente.RepresentanteId);

            return new ClienteDTO
            {
                Id = cliente.Id,
                NomeRazaoSocial = cliente.NomeRazaoSocial,
                CpfCnpj = cliente.CpfCnpj,
                RepresentanteId = cliente.RepresentanteId,
                NomeRepresentante = representante?.Nome,
                StatusCliente = cliente.StatusCliente
            };
        }
    }
}

