using ServicoOportunidades.DTOs;
using System.Text.Json;

namespace ServicoOportunidades.Services
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

        public async Task<ClienteDTO?> ObterClienteAsync(int id)
        {
            var url = $"{_configuration["ServicoClientes:BaseUrl"]}/api/clientes/{id}";
            var response = await _httpClient.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ClienteDTO>(content, new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                });
            }
            
            return null;
        }

        public async Task<bool> AtualizarStatusClienteAsync(int clienteId, string status)
        {
            var url = $"{_configuration["ServicoClientes:BaseUrl"]}/api/clientes/{clienteId}/status";
            var dto = new { StatusCliente = status };
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PatchAsync(url, content);
            return response.IsSuccessStatusCode;
        }
    }

    // DTOs para integração
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TipoPerfil { get; set; } = string.Empty;
    }

    public class ClienteDTO
    {
        public int Id { get; set; }
        public string NomeRazaoSocial { get; set; } = string.Empty;
        public string CpfCnpj { get; set; } = string.Empty;
        public int RepresentanteId { get; set; }
        public string StatusCliente { get; set; } = string.Empty;
    }
}

