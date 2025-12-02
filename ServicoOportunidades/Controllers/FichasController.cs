using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicoOportunidades.DTOs;
using ServicoOportunidades.Data;
using ServicoOportunidades.Services;

namespace ServicoOportunidades.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FichasController : ControllerBase
    {
        private readonly IFichaService _fichaService;
        private readonly OportunidadesContext _context;

        public FichasController(IFichaService fichaService, OportunidadesContext context)
        {
            _fichaService = fichaService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CriarFicha([FromBody] CriarFichaDTO dto)
        {
            try
            {
                var ficha = await _fichaService.CriarFichaAsync(dto);
                return CreatedAtAction(nameof(ObterFicha), new { id = ficha.Id }, ficha);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterFicha(int id)
        {
            var ficha = await _fichaService.ObterFichaPorIdAsync(id);
            if (ficha == null)
                return NotFound();

            return Ok(ficha);
        }

        [HttpGet]
        public async Task<IActionResult> ListarFichas()
        {
            var fichas = await _fichaService.ListarFichasAsync();
            return Ok(fichas);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> AtualizarStatus(int id, [FromBody] AtualizarStatusFichaDTO dto)
        {
            try
            {
                var ficha = await _fichaService.AtualizarStatusAsync(id, dto);
                if (ficha == null)
                    return NotFound();

                return Ok(ficha);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("cliente/{clienteId}/cancelar")]
        public async Task<IActionResult> CancelarFichasPorCliente(int clienteId)
        {
            try
            {
                var fichasCanceladas = await _fichaService.CancelarFichasPorClienteAsync(clienteId);
                return Ok(new { mensagem = $"{fichasCanceladas} ficha(s) cancelada(s) com sucesso", fichasCanceladas });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                var totalFichas = await _context.Fichas.CountAsync();
                var fichasVendidas = await _context.Fichas.CountAsync(f => f.StatusFicha == "Vendido");
                var fichasEmCadastro = await _context.Fichas.CountAsync(f => f.StatusFicha == "Em Cadastro");
                var fichasEmAnalise = await _context.Fichas.CountAsync(f => f.StatusFicha == "Em Análise");
                var fichasCanceladas = await _context.Fichas.CountAsync(f => f.StatusFicha == "Cancelado");

                var fichasPorRepresentante = await _context.Fichas
                    .GroupBy(f => f.RepresentanteId)
                    .Select(g => new
                    {
                        representanteId = g.Key,
                        quantidade = g.Count(),
                        vendidas = g.Count(f => f.StatusFicha == "Vendido")
                    })
                    .OrderByDescending(x => x.quantidade)
                    .Take(5)
                    .ToListAsync();

                var taxaFechamento = totalFichas > 0
                    ? Math.Round((double)fichasVendidas / totalFichas * 100, 2)
                    : 0;

                var valorTotalEstimado = await _context.Fichas
                    .Where(f => f.ValorEstimado.HasValue)
                    .SumAsync(f => f.ValorEstimado) ?? 0;

                var valorVendido = await _context.Fichas
                    .Where(f => f.StatusFicha == "Vendido" && f.ValorEstimado.HasValue)
                    .SumAsync(f => f.ValorEstimado) ?? 0;

                var porStatus = new
                {
                    emCadastro = fichasEmCadastro,
                    emAnalise = fichasEmAnalise,
                    vendido = fichasVendidas,
                    cancelado = fichasCanceladas
                };

                var dashboard = new
                {
                    resumo = new
                    {
                        total = totalFichas,
                        vendidas = fichasVendidas,
                        emCadastro = fichasEmCadastro,
                        emAnalise = fichasEmAnalise,
                        canceladas = fichasCanceladas,
                        taxaFechamento = taxaFechamento
                    },
                    financeiro = new
                    {
                        valorTotalEstimado = Math.Round(valorTotalEstimado, 2),
                        valorVendido = Math.Round(valorVendido, 2),
                        percentualVendido = totalFichas > 0 ? Math.Round((double)fichasVendidas / totalFichas * 100, 2) : 0
                    },
                    topRepresentantes = fichasPorRepresentante,
                    porStatus = porStatus
                };

                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

