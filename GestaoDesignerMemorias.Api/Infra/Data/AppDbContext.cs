using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerMemorias.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        //public DbSet<Cliente> Clientes => Set<Cliente>();
        //public DbSet<Pedido> Pedidos => Set<Pedido>();
        //public DbSet<BriefingItem> BriefingItems => Set<BriefingItem>();
        //public DbSet<MensagemWebhook> MensagensWebhook => Set<MensagemWebhook>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //// Cliente
            //modelBuilder.Entity<Cliente>(entity =>
            //{
            //    entity.HasKey(c => c.Id);
            //    entity.HasIndex(c => c.Telefone).IsUnique();
            //    entity.Property(c => c.Nome).HasMaxLength(200);
            //    entity.Property(c => c.Telefone).IsRequired().HasMaxLength(20);
            //});

            //// Pedido
            //modelBuilder.Entity<Pedido>(entity =>
            //{
            //    entity.HasKey(p => p.Id);

            //    entity.HasOne(p => p.Cliente)
            //          .WithMany(c => c.Pedidos)
            //          .HasForeignKey(p => p.ClienteId);

            //    entity.Property(p => p.ValorTotal)
            //          .HasPrecision(10, 2);

            //    entity.Property(p => p.ValorPago)
            //          .HasPrecision(10, 2);
            //});

            //// BriefingItem
            //modelBuilder.Entity<BriefingItem>(entity =>
            //{
            //    entity.HasKey(b => b.Id);

            //    entity.HasOne(b => b.Pedido)
            //          .WithMany(p => p.BriefingItems)
            //          .HasForeignKey(b => b.PedidoId);

            //    entity.Property(b => b.Pergunta).IsRequired();
            //});

            //// MensagemWebhook
            //modelBuilder.Entity<MensagemWebhook>(entity =>
            //{
            //    entity.HasKey(m => m.Id);
            //    entity.Property(m => m.TelefoneOrigem).IsRequired();
            //    entity.Property(m => m.Conteudo).IsRequired();
            //});
        }
    }
}
