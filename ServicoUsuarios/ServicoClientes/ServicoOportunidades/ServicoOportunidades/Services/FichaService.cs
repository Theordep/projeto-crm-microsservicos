using Microsoft.EntityFrameworkCore;
using ServicoOportunidades.Data;
using ServicoOportunidades.DTOs;
using ServicoOportunidades.Models;

namespace ServicoOportunidades.Services
{
    public class FichaService : IFichaService
    {
        private readonly OportunidadesContext _context;
        private readonly IntegracaoService _integracaoService;

        public FichaService(OportunidadesContext context, IntegracaoService integracaoService)
        {
            _context = context;
            _integracaoService = integracaoService;
        }

        public async Task<FichaDTO?> CriarFichaAsync(CriarFichaDTO dto)
        {
            // Integração a.1: Validar se representante existe
            var representante = await _integracaoService.ObterUsuarioAsync(dto.RepresentanteId);
            if (representante == null)
            {
                throw new Exception("Representante não encontrado");
            }

            // Integração a.2: Validar se cliente existe e buscar nome
            var cliente = await _integracaoService.ObterClienteAsync(dto.ClienteId);
            if (cliente == null)
            {
                throw new Exception("Cliente não encontrado");
            }

            var ficha = new Ficha
            {
                ClienteId = dto.ClienteId,
                RepresentanteId = dto.RepresentanteId,
                StatusFicha = "Em Cadastro",
                TituloObra = dto.TituloObra,
                DescricaoSimples = dto.DescricaoSimples,
                ValorEstimado = dto.ValorEstimado,
                AreaM2 = dto.AreaM2
            };

            _context.Fichas.Add(ficha);
            await _context.SaveChangesAsync();

            return new FichaDTO
            {
                Id = ficha.Id,
                ClienteId = ficha.ClienteId,
                NomeCliente = cliente.NomeRazaoSocial,
                RepresentanteId = ficha.RepresentanteId,
                NomeRepresentante = representante.Nome,
                StatusFicha = ficha.StatusFicha,
                TituloObra = ficha.TituloObra,
                DescricaoSimples = ficha.DescricaoSimples,
                ValorEstimado = ficha.ValorEstimado,
                AreaM2 = ficha.AreaM2
            };
        }

        public async Task<FichaDTO?> ObterFichaPorIdAsync(int id)
        {
            var ficha = await _context.Fichas.FindAsync(id);
            if (ficha == null) return null;

            // Buscar dados do cliente e representante
            var cliente = await _integracaoService.ObterClienteAsync(ficha.ClienteId);
            var representante = await _integracaoService.ObterUsuarioAsync(ficha.RepresentanteId);

            return new FichaDTO
            {
                Id = ficha.Id,
                ClienteId = ficha.ClienteId,
                NomeCliente = cliente?.NomeRazaoSocial,
                RepresentanteId = ficha.RepresentanteId,
                NomeRepresentante = representante?.Nome,
                StatusFicha = ficha.StatusFicha,
                TituloObra = ficha.TituloObra,
                DescricaoSimples = ficha.DescricaoSimples,
                ValorEstimado = ficha.ValorEstimado,
                AreaM2 = ficha.AreaM2
            };
        }

        public async Task<List<FichaDTO>> ListarFichasAsync()
        {
            var fichas = await _context.Fichas.ToListAsync();
            var fichasDTO = new List<FichaDTO>();

            foreach (var ficha in fichas)
            {
                var cliente = await _integracaoService.ObterClienteAsync(ficha.ClienteId);
                var representante = await _integracaoService.ObterUsuarioAsync(ficha.RepresentanteId);

                fichasDTO.Add(new FichaDTO
                {
                    Id = ficha.Id,
                    ClienteId = ficha.ClienteId,
                    NomeCliente = cliente?.NomeRazaoSocial,
                    RepresentanteId = ficha.RepresentanteId,
                    NomeRepresentante = representante?.Nome,
                    StatusFicha = ficha.StatusFicha,
                    TituloObra = ficha.TituloObra,
                    DescricaoSimples = ficha.DescricaoSimples,
                    ValorEstimado = ficha.ValorEstimado,
                    AreaM2 = ficha.AreaM2
                });
            }

            return fichasDTO;
        }

        public async Task<FichaDTO?> AtualizarStatusAsync(int id, AtualizarStatusFichaDTO dto)
        {
            var ficha = await _context.Fichas.FindAsync(id);
            if (ficha == null) return null;

            var statusAnterior = ficha.StatusFicha;
            ficha.StatusFicha = dto.StatusFicha;
            await _context.SaveChangesAsync();

            // Integração b: Se status mudou para "Vendido", atualizar cliente para "Ativo"
            if (statusAnterior != "Vendido" && dto.StatusFicha == "Vendido")
            {
                await _integracaoService.AtualizarStatusClienteAsync(ficha.ClienteId, "Ativo");
            }

            // Buscar dados atualizados
            var cliente = await _integracaoService.ObterClienteAsync(ficha.ClienteId);
            var representante = await _integracaoService.ObterUsuarioAsync(ficha.RepresentanteId);

            return new FichaDTO
            {
                Id = ficha.Id,
                ClienteId = ficha.ClienteId,
                NomeCliente = cliente?.NomeRazaoSocial,
                RepresentanteId = ficha.RepresentanteId,
                NomeRepresentante = representante?.Nome,
                StatusFicha = ficha.StatusFicha,
                TituloObra = ficha.TituloObra,
                DescricaoSimples = ficha.DescricaoSimples,
                ValorEstimado = ficha.ValorEstimado,
                AreaM2 = ficha.AreaM2
            };
        }
    }
}

