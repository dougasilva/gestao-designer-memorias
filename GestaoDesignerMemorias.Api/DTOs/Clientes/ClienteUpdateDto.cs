using System.ComponentModel.DataAnnotations;

namespace GestaoDesignerMemorias.DTOs.Clientes;

public class ClienteUpdateDto
{
    [Required]
    [MaxLength(150)]
    public string Nome { get; set; } = null!;

    [Required]
    [MaxLength(20)]
    public string Telefone { get; set; } = null!;

    [EmailAddress]
    [MaxLength(150)]
    public string? Email { get; set; }
}
