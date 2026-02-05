namespace GestaoDesignerMemorias.DTOs.Pedidos;

public class PedidoTimelineDto
{
    public string Tipo { get; set; } = null!;
    public string? Descricao { get; set; }
    public DateTime CriadoEm { get; set; }
}
