using GestaoDesignerMemorias.Domain.Enums;

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
        public BriefingItemType Tipo { get; set; }

        // usado quando Tipo = OpcaoUnica ou OpcaoMultipla
        public string? Opcoes { get; set; } // CSV ou JSON simples
    }
}
