namespace ServicoOportunidades.DTOs
{
    public class FichaDTO
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string? NomeCliente { get; set; }
        public int RepresentanteId { get; set; }
        public string? NomeRepresentante { get; set; }
        public string StatusFicha { get; set; } = string.Empty;
        public string TituloObra { get; set; } = string.Empty;
        public string? DescricaoSimples { get; set; }
        public double? ValorEstimado { get; set; }
        public double? AreaM2 { get; set; }
    }

    public class CriarFichaDTO
    {
        public int ClienteId { get; set; }
        public int RepresentanteId { get; set; }
        public string TituloObra { get; set; } = string.Empty;
        public string? DescricaoSimples { get; set; }
        public double? ValorEstimado { get; set; }
        public double? AreaM2 { get; set; }
    }

    public class AtualizarStatusFichaDTO
    {
        public string StatusFicha { get; set; } = string.Empty;
    }
}

