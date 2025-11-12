using ServicoClientes.DTOs;
using System.Text.Json;

namespace ServicoClientes.Services
{
    public class IntegracaoService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public IntegracaoService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        // Consulta 1: Buscar dados do representante (usuário)
        public async Task<UsuarioDTO?> ObterUsuarioAsync(int id)
        {
            var url = $"{_configuration["ServicoUsuarios:BaseUrl"]}/api/usuarios/{id}";
            var response = await _httpClient.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UsuarioDTO>(content, new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                });
            }
            
            return null;
        }

        // Alteração: Cancelar fichas de um cliente no ServicoOportunidades
        public async Task<bool> CancelarFichasPorClienteAsync(int clienteId)
        {
            var url = $"{_configuration["ServicoOportunidades:BaseUrl"]}/api/fichas/cliente/{clienteId}/cancelar";
            var response = await _httpClient.PostAsync(url, null);
            return response.IsSuccessStatusCode;
        }
    }

    // DTO para integração com ServicoUsuarios
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TipoPerfil { get; set; } = string.Empty;
    }
}

