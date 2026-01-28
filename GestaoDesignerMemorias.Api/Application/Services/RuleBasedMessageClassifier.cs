using GestaoDesignerMemorias.Domain.Enums;

namespace Application.Services;

public class RuleBasedMessageClassifier : IMessageClassifier
{
    public TipoIntencaoMensagem Classificar(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return TipoIntencaoMensagem.Outros;

        var text = message.ToLowerInvariant();

        if (ContainsAny(text, "orçamento", "orcamento", "preço", "valor"))
            return TipoIntencaoMensagem.Orcamento;

        if (ContainsAny(text, "pedido", "encomenda", "quero fazer", "comprar"))
            return TipoIntencaoMensagem.Pedido;

        if (ContainsAny(text, "problema", "erro", "não funciona", "falha"))
            return TipoIntencaoMensagem.Suporte;

        if (ContainsAny(text, "dúvida", "duvida", "como", "quando"))
            return TipoIntencaoMensagem.Duvida;

        return TipoIntencaoMensagem.Outros;
    }

    private static bool ContainsAny(string text, params string[] keywords)
        => keywords.Any(k => text.Contains(k));
}
