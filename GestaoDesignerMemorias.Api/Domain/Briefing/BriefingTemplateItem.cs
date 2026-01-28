using GestaoDesignerMemorias.Domain.Enums;

namespace GestaoDesignerMemorias.Domain.Briefing;

public class BriefingTemplateItem
{
    public string Pergunta { get; set; } = null!;
    public BriefingItemType Tipo { get; set; }
    public string? Opcoes { get; set; }
}
