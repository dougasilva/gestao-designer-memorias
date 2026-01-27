using System.ComponentModel.DataAnnotations;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Domain.Enums;

namespace GestaoDesignerMemorias.DTOs.Pedidos;

public class PedidoUpdateDto
{
    [Required]
    public StatusPedido Status { get; set; }

    public StatusPagamento StatusPagamento { get; set; }
}
