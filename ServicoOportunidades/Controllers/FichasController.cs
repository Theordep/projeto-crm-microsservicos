using Microsoft.AspNetCore.Mvc;
using ServicoOportunidades.DTOs;
using ServicoOportunidades.Services;

namespace ServicoOportunidades.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FichasController : ControllerBase
    {
        private readonly IFichaService _fichaService;

        public FichasController(IFichaService fichaService)
        {
            _fichaService = fichaService;
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
    }
}

