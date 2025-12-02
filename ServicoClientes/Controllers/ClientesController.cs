using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicoClientes.DTOs;
using ServicoClientes.Data;
using ServicoClientes.Services;

namespace ServicoClientes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly ClientesContext _context;

        public ClientesController(IClienteService clienteService, ClientesContext context)
        {
            _clienteService = clienteService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CriarCliente([FromBody] CriarClienteDTO dto)
        {
            try
            {
                var cliente = await _clienteService.CriarClienteAsync(dto);
                return CreatedAtAction(nameof(ObterCliente), new { id = cliente.Id }, cliente);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterCliente(int id)
        {
            var cliente = await _clienteService.ObterClientePorIdAsync(id);
            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }

        [HttpGet]
        public async Task<IActionResult> ListarClientes()
        {
            var clientes = await _clienteService.ListarClientesAsync();
            return Ok(clientes);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> AtualizarStatus(int id, [FromBody] AtualizarStatusClienteDTO dto)
        {
            try
            {
                var cliente = await _clienteService.AtualizarStatusAsync(id, dto);
                if (cliente == null)
                    return NotFound();

                return Ok(cliente);
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
                // Totalizadores
                var totalClientes = await _context.Clientes.CountAsync();
                var clientesAtivos = await _context.Clientes.CountAsync(c => c.StatusCliente == "Ativo");
                var clientesProspect = await _context.Clientes.CountAsync(c => c.StatusCliente == "Prospect");

                // Clientes por representante (Top 5)
                var clientesPorRepresentante = await _context.Clientes
                    .GroupBy(c => c.RepresentanteId)
                    .Select(g => new
                    {
                        representanteId = g.Key,
                        quantidade = g.Count()
                    })
                    .OrderByDescending(x => x.quantidade)
                    .Take(5)
                    .ToListAsync();

                // Clientes cadastrados nos últimos 7 dias
                // Nota: Como não temos campo de data, usaremos contagem total
                var clientesRecentes = clientesAtivos;

                // Taxa de conversão
                var taxaConversao = totalClientes > 0
                    ? Math.Round((double)clientesAtivos / totalClientes * 100, 2)
                    : 0;

                var dashboard = new
                {
                    resumo = new
                    {
                        total = totalClientes,
                        ativos = clientesAtivos,
                        prospects = clientesProspect,
                        taxaConversao = taxaConversao
                    },
                    topRepresentantes = clientesPorRepresentante,
                    porStatus = new
                    {
                        ativo = clientesAtivos,
                        prospect = clientesProspect
                    }
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

