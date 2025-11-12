namespace ServicoClientes.DTOs
{
    public class ClienteDTO
    {
        public int Id { get; set; }
        public string NomeRazaoSocial { get; set; } = string.Empty;
        public string CpfCnpj { get; set; } = string.Empty;
        public int RepresentanteId { get; set; }
        public string? NomeRepresentante { get; set; }
        public string StatusCliente { get; set; } = string.Empty;
    }

    public class CriarClienteDTO
    {
        public string NomeRazaoSocial { get; set; } = string.Empty;
        public string CpfCnpj { get; set; } = string.Empty;
        public int RepresentanteId { get; set; }
    }

    public class AtualizarStatusClienteDTO
    {
        public string StatusCliente { get; set; } = string.Empty;
    }
}

