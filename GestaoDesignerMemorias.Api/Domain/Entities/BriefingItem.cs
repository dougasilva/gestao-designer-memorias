namespace GestaoDesignerMemorias.Domain.Entities
{
    public class BriefingItem
    {
        public Guid Id { get; set; }

        public Guid PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;

        public string Pergunta { get; set; } = null!;
        public string? Resposta { get; set; }
        public int Ordem { get; set; }
    }
}
