using System.ComponentModel.DataAnnotations;

namespace GestaoDesignerMemorias.DTOs.Pedidos;

public class PedidoCreateDto
{
    [Required]
    public Guid ClienteId { get; set; }

    [Required]
    [MaxLength(100)]
    public string TipoEvento { get; set; } = null!;

    [Required]
    public decimal ValorTotal { get; set; }
}
