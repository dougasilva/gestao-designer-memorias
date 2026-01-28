using GestaoDesignerMemorias.Domain.Briefing;
using GestaoDesignerMemorias.Domain.Enums;

namespace GestaoDesignerMemorias.Tests.Domain.Briefing;

public class BriefingTemplateFactoryTests
{
    [Theory]
    [InlineData("aniversário")]
    [InlineData("Aniversario")]
    [InlineData(" ANIVERSÁRIO ")]
    public void Criar_DeveReconhecerAniversario_IndependenteDoTexto(string tipoEvento)
    {
        var itens = BriefingTemplateFactory.Criar(tipoEvento);

        Assert.Contains(itens, i => i.Pergunta == "Tema do evento");
    }

}
