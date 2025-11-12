namespace ServicoOportunidades.Models
{
    public class Ficha
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int RepresentanteId { get; set; }
        public string StatusFicha { get; set; } = "Em Cadastro"; // 'Em Cadastro', 'Em Análise', 'Vendido', 'Cancelado'
        public string TituloObra { get; set; } = string.Empty;
        public string? DescricaoSimples { get; set; }
        public double? ValorEstimado { get; set; }
        public double? AreaM2 { get; set; }
    }
}

