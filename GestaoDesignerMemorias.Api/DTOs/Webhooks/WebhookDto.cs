using System.ComponentModel.DataAnnotations;

namespace GestaoDesignerMemorias.DTOs.Webhooks;

public class WebhookDto
{
    [Required]
    public string Telefone { get; set; } = null!;

    public string? Nome { get; set; }

    [Required]
    public string Mensagem { get; set; } = null!;
}
