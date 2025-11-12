using Microsoft.EntityFrameworkCore;
using ServicoUsuarios.Data;
using ServicoUsuarios.DTOs;
using ServicoUsuarios.Models;
using System.Security.Cryptography;
using System.Text;

namespace ServicoUsuarios.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UsuariosContext _context;

        public UsuarioService(UsuariosContext context)
        {
            _context = context;
        }

        public async Task<UsuarioDTO?> CriarUsuarioAsync(CriarUsuarioDTO dto)
        {
            // Verificar se email já existe
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
            {
                throw new Exception("Email já cadastrado");
            }

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = HashSenha(dto.Senha),
                TipoPerfil = dto.TipoPerfil
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                TipoPerfil = usuario.TipoPerfil
            };
        }

        public async Task<UsuarioDTO?> ObterUsuarioPorIdAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return null;

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                TipoPerfil = usuario.TipoPerfil
            };
        }

        public async Task<UsuarioDTO?> ObterUsuarioPorEmailAsync(string email)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null) return null;

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                TipoPerfil = usuario.TipoPerfil
            };
        }

        public async Task<List<UsuarioDTO>> ListarUsuariosAsync()
        {
            return await _context.Usuarios
                .Select(u => new UsuarioDTO
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    TipoPerfil = u.TipoPerfil
                })
                .ToListAsync();
        }

        private string HashSenha(string senha)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(senha);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}

