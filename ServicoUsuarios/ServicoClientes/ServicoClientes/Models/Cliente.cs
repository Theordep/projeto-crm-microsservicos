namespace ServicoClientes.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string NomeRazaoSocial { get; set; } = string.Empty;
        public string CpfCnpj { get; set; } = string.Empty;
        public int RepresentanteId { get; set; }
        public string StatusCliente { get; set; } = "Prospect"; // 'Prospect' ou 'Ativo'
    }
}

