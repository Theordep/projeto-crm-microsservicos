using ServicoOportunidades.DTOs;

namespace ServicoOportunidades.Services
{
    public interface IFichaService
    {
        Task<FichaDTO?> CriarFichaAsync(CriarFichaDTO dto);
        Task<FichaDTO?> ObterFichaPorIdAsync(int id);
        Task<List<FichaDTO>> ListarFichasAsync();
        Task<FichaDTO?> AtualizarStatusAsync(int id, AtualizarStatusFichaDTO dto);
    }
}

