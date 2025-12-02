using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicoUsuarios.DTOs;
using ServicoUsuarios.Data;
using ServicoUsuarios.Services;

namespace ServicoUsuarios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly UsuariosContext _context;

        public UsuariosController(IUsuarioService usuarioService, UsuariosContext context)
        {
            _usuarioService = usuarioService;
            _context = context;
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

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                // Totalizadores
                var totalUsuarios = await _context.Usuarios.CountAsync();
                var usuariosRepresentantes = await _context.Usuarios.CountAsync(u => u.TipoPerfil == "Representante");
                var usuariosComercial = await _context.Usuarios.CountAsync(u => u.TipoPerfil == "Comercial");

                // Distribuição por perfil
                var porPerfil = new
                {
                    representante = usuariosRepresentantes,
                    comercial = usuariosComercial
                };

                var dashboard = new
                {
                    resumo = new
                    {
                        total = totalUsuarios,
                        representantes = usuariosRepresentantes,
                        comerciais = usuariosComercial
                    },
                    porPerfil = porPerfil,
                    percentualPorPerfil = new
                    {
                        representante = totalUsuarios > 0 ? Math.Round((double)usuariosRepresentantes / totalUsuarios * 100, 2) : 0,
                        comercial = totalUsuarios > 0 ? Math.Round((double)usuariosComercial / totalUsuarios * 100, 2) : 0
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

