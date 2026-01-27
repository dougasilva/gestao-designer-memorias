namespace GestaoDesignerMemorias.Domain.Entities
{
    public class Cliente
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string Telefone { get; set; } = null!;
        public string? Email { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
