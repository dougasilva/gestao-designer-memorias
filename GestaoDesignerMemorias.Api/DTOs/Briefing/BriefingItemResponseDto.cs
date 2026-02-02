using GestaoDesignerMemorias.Domain.Enums;

namespace GestaoDesignerMemorias.Api.DTOs.Briefing;

public class BriefingItemResponseDto
{
    public Guid Id { get; set; }
    public string Pergunta { get; set; } = null!;
    public string? Resposta { get; set; }
    public int Ordem { get; set; }
    public BriefingItemType Tipo { get; set; }
    public string? Opcoes { get; set; }
}
