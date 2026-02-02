namespace GestaoDesignerMemorias.Domain.Entities
{
    public class MensagemWebhook
    {
        public Guid Id { get; set; }

        public string TelefoneOrigem { get; set; } = null!;
        public string Conteudo { get; set; } = null!;

        public DateTime RecebidoEm { get; set; } = DateTime.UtcNow;

        public bool Processado { get; set; }
        public string? Observacao { get; set; }
    }

}
