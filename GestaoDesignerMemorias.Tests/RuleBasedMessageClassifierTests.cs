using Application.Services;
using GestaoDesignerMemorias.Domain.Enums;

namespace GestaoDesignerMemorias.Tests;

public class RuleBasedMessageClassifierTests
{
    private readonly IMessageClassifier _classifier;

    public RuleBasedMessageClassifierTests()
    {
        _classifier = new RuleBasedMessageClassifier();
    }

    [Theory]
    [InlineData("quero um orçamento", MessageCategory.Orcamento)]
    [InlineData("qual o valor?", MessageCategory.Orcamento)]
    [InlineData("preço do serviço", MessageCategory.Orcamento)]
    public void Deve_Classificar_Orcamento(string mensagem, MessageCategory esperado)
    {
        var resultado = _classifier.Classify(mensagem);
        Assert.Equal(esperado, resultado);
    }

    [Theory]
    [InlineData("quero fazer um pedido", MessageCategory.Pedido)]
    [InlineData("gostaria de encomendar", MessageCategory.Pedido)]
    public void Deve_Classificar_Pedido(string mensagem, MessageCategory esperado)
    {
        var resultado = _classifier.Classify(mensagem);
        Assert.Equal(esperado, resultado);
    }

    [Theory]
    [InlineData("não funciona", MessageCategory.Suporte)]
    [InlineData("deu erro", MessageCategory.Suporte)]
    public void Deve_Classificar_Suporte(string mensagem, MessageCategory esperado)
    {
        var resultado = _classifier.Classify(mensagem);
        Assert.Equal(esperado, resultado);
    }

    [Fact]
    public void Mensagem_Desconhecida_Deve_Ser_Outros()
    {
        var resultado = _classifier.Classify("oi tudo bem");
        Assert.Equal(MessageCategory.Outros, resultado);
    }
}
