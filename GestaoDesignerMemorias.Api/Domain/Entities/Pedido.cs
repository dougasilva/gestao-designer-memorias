using GestaoDesignerMemorias.Domain.Enums;

namespace GestaoDesignerMemorias.Domain.Entities
{
    public class Pedido
    {
        public Guid Id { get; set; }

        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;

        public string TipoEvento { get; set; } = null!;

        public StatusPedido Status { get; set; } = StatusPedido.Prospecao;
        public StatusPagamento StatusPagamento { get; set; } = StatusPagamento.Nenhum;

        public decimal ValorTotal { get; set; }
        public decimal ValorPago { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataEvento { get; set; }

        public ICollection<BriefingItem> BriefingItens { get; set; } = new List<BriefingItem>();

        public MarcoPedido Marco { get; set; } = MarcoPedido.Criado;

    }
}
