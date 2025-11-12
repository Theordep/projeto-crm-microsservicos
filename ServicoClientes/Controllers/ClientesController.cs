using Microsoft.AspNetCore.Mvc;
using ServicoClientes.DTOs;
using ServicoClientes.Services;

namespace ServicoClientes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
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
    }
}

