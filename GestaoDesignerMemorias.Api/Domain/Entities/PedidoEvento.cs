namespace GestaoDesignerMemorias.Domain.Entities
{
    public class PedidoEvento
    {
        public Guid Id { get; set; }

        public Guid PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;

        public string Tipo { get; set; } = null!;
        // ex: "PedidoCriado", "BriefingIniciado", "BriefingConcluido", "StatusAlterado"

        public string? Descricao { get; set; }

        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}
