using GestaoDesignerMemorias.Domain.Enums;

namespace GestaoDesignerMemorias.Domain.Briefing;

public static class BriefingTemplateFactory
{
    public static List<BriefingTemplateItem> Criar(string tipoEvento)
    {
        if (string.IsNullOrWhiteSpace(tipoEvento))
            return Generico();

        return tipoEvento.Trim().ToLower() switch
        {
            "aniversário" => Aniversario(),
            "aniversario" => Aniversario(),
            "casamento" => Casamento(),
            _ => Generico()
        };
    }

    private static List<BriefingTemplateItem> Aniversario() => new()
    {
        new() { Pergunta = "Tema do evento", Tipo = BriefingItemType.Texto },
        new() { Pergunta = "Data do evento", Tipo = BriefingItemType.Data },
        new() { Pergunta = "Quantidade de convidados", Tipo = BriefingItemType.Numero }
    };

    private static List<BriefingTemplateItem> Casamento() => new()
    {
        new() { Pergunta = "Nome dos noivos", Tipo = BriefingItemType.Texto },
        new() { Pergunta = "Data do casamento", Tipo = BriefingItemType.Data },
        new()
        {
            Pergunta = "Estilo desejado",
            Tipo = BriefingItemType.OpcaoUnica,
            Opcoes = "Clássico,Moderno,Rústico"
        }
    };

    private static List<BriefingTemplateItem> Generico() => new()
    {
        new() { Pergunta = "Descreva o evento", Tipo = BriefingItemType.Texto },
        new() { Pergunta = "Tem local definido", Tipo = BriefingItemType.Texto },
        new() { Pergunta = "Como nso conheceu", Tipo = BriefingItemType.Texto }
    };
}
