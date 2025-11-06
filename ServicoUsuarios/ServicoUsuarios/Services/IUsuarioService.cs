using ServicoUsuarios.DTOs;

namespace ServicoUsuarios.Services
{
    public interface IUsuarioService
    {
        Task<UsuarioDTO?> CriarUsuarioAsync(CriarUsuarioDTO dto);
        Task<UsuarioDTO?> ObterUsuarioPorIdAsync(int id);
        Task<UsuarioDTO?> ObterUsuarioPorEmailAsync(string email);
        Task<List<UsuarioDTO>> ListarUsuariosAsync();
    }
}

