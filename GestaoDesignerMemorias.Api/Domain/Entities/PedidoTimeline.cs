using GestaoDesignerMemorias.Domain.Enums;

namespace GestaoDesignerMemorias.Domain.Entities;

public class PedidoTimeline
{
    public Guid Id { get; set; }

    public Guid PedidoId { get; set; }
    public Pedido Pedido { get; set; } = null!;

    public StatusPedido Status { get; set; }
    public MarcoPedido Marco { get; set; }

    public string Evento { get; set; } = null!;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
