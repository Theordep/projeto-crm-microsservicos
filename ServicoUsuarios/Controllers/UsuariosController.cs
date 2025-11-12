using Microsoft.AspNetCore.Mvc;
using ServicoUsuarios.DTOs;
using ServicoUsuarios.Services;

namespace ServicoUsuarios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioDTO dto)
        {
            try
            {
                var usuario = await _usuarioService.CriarUsuarioAsync(dto);
                return CreatedAtAction(nameof(ObterUsuario), new { id = usuario.Id }, usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterUsuario(int id)
        {
            var usuario = await _usuarioService.ObterUsuarioPorIdAsync(id);
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpGet]
        public async Task<IActionResult> ListarUsuarios()
        {
            var usuarios = await _usuarioService.ListarUsuariosAsync();
            return Ok(usuarios);
        }
    }
}

