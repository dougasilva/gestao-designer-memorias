using GestaoDesignerMemorias.Domain.Enums;

namespace Application.Services
{
    public class RuleBasedMessageClassifier : IMessageClassifier
    {
        public MessageCategory Classify(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return MessageCategory.Outros;

            var text = message.ToLowerInvariant();

            if (ContainsAny(text, "orçamento", "orcamento", "preço", "valor"))
                return MessageCategory.Orcamento;

            if (ContainsAny(text, "pedido", "encomenda", "quero fazer", "comprar"))
                return MessageCategory.Pedido;

            if (ContainsAny(text, "problema", "erro", "não funciona", "falha"))
                return MessageCategory.Suporte;

            if (ContainsAny(text, "dúvida", "duvida", "como", "quando"))
                return MessageCategory.Duvida;

            return MessageCategory.Outros;
        }

        private static bool ContainsAny(string text, params string[] keywords)
            => keywords.Any(k => text.Contains(k));
    }
}
