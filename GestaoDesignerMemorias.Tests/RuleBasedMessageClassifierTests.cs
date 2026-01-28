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
    [InlineData("quero um orçamento", TipoIntencaoMensagem.Orcamento)]
    [InlineData("qual o valor?", TipoIntencaoMensagem.Orcamento)]
    [InlineData("preço do serviço", TipoIntencaoMensagem.Orcamento)]
    public void Deve_Classificar_Orcamento(string mensagem, TipoIntencaoMensagem esperado)
    {
        var resultado = _classifier.Classificar(mensagem);
        Assert.Equal(esperado, resultado);
    }

    [Theory]
    [InlineData("quero fazer um pedido", TipoIntencaoMensagem.Pedido)]
    [InlineData("gostaria de encomendar", TipoIntencaoMensagem.Pedido)]
    public void Deve_Classificar_Pedido(string mensagem, TipoIntencaoMensagem esperado)
    {
        var resultado = _classifier.Classificar(mensagem);
        Assert.Equal(esperado, resultado);
    }

    [Theory]
    [InlineData("não funciona", TipoIntencaoMensagem.Suporte)]
    [InlineData("deu erro", TipoIntencaoMensagem.Suporte)]
    public void Deve_Classificar_Suporte(string mensagem, TipoIntencaoMensagem esperado)
    {
        var resultado = _classifier.Classificar(mensagem);
        Assert.Equal(esperado, resultado);
    }

    [Fact]
    public void Mensagem_Desconhecida_Deve_Ser_Outros()
    {
        var resultado = _classifier.Classificar("oi tudo bem");
        Assert.Equal(TipoIntencaoMensagem.Outros, resultado);
    }
}
