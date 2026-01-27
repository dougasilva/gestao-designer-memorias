namespace GestaoDesignerMemorias.DTOs.Clientes;

public class ClienteResponseDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Telefone { get; set; } = null!;
    public string? Email { get; set; }
}
