using GestaoDesignerMemorias.Domain.Enums;

namespace GestaoDesignerMemorias.DTOs.Pedidos;

public class PedidoResponseDto
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public string TipoEvento { get; set; } = null!;
    public decimal ValorTotal { get; set; }
    public StatusPedido Status { get; set; }
    public StatusPagamento StatusPagamento { get; set; }
    public DateTime DataCriacao { get; set; }
}
