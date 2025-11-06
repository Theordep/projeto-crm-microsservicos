using ServicoClientes.DTOs;

namespace ServicoClientes.Services
{
    public interface IClienteService
    {
        Task<ClienteDTO?> CriarClienteAsync(CriarClienteDTO dto);
        Task<ClienteDTO?> ObterClientePorIdAsync(int id);
        Task<List<ClienteDTO>> ListarClientesAsync();
        Task<ClienteDTO?> AtualizarStatusAsync(int id, AtualizarStatusClienteDTO dto);
    }
}

