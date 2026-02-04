using GestaoDesignerMemorias.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerMemorias.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<BriefingItem> BriefingItens => Set<BriefingItem>();
        public DbSet<MensagemWebhook> MensagensWebhook => Set<MensagemWebhook>();
        public DbSet<PedidoEvento> PedidoEventos => Set<PedidoEvento>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // Cliente
            // =========================
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.HasIndex(c => c.Telefone)
                      .IsUnique();

                entity.Property(c => c.Nome)
                      .HasMaxLength(200);

                entity.Property(c => c.Telefone)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(c => c.Email)
                      .HasMaxLength(200);

                entity.Property(c => c.DataCriacao)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // =========================
            // Pedido
            // =========================
            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.HasOne(p => p.Cliente)
                      .WithMany(c => c.Pedidos)
                      .HasForeignKey(p => p.ClienteId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(p => p.ClienteId);

                entity.Property(p => p.TipoEvento)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(p => p.Status)
                      .HasConversion<int>();

                entity.Property(p => p.StatusPagamento)
                      .HasConversion<int>();

                entity.Property(p => p.Marco)
                      .HasConversion<int>();

                entity.Property(p => p.ValorTotal)
                      .HasPrecision(10, 2);

                entity.Property(p => p.ValorPago)
                      .HasPrecision(10, 2);

                entity.Property(p => p.DataCriacao)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // =========================
            // BriefingItem
            // =========================
            modelBuilder.Entity<BriefingItem>(entity =>
            {
                entity.HasKey(b => b.Id);

                entity.HasOne(b => b.Pedido)
                      .WithMany(p => p.BriefingItens)
                      .HasForeignKey(b => b.PedidoId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(b => new { b.PedidoId, b.Ordem });

                entity.Property(b => b.Pergunta)
                      .IsRequired();

                entity.Property(b => b.Tipo)
                      .HasConversion<int>();

                entity.Property(b => b.Opcoes)
                      .HasMaxLength(1000);
            });

            // =========================
            // MensagemWebhook
            // =========================
            modelBuilder.Entity<MensagemWebhook>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.TelefoneOrigem)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(m => m.Conteudo)
                      .IsRequired();

                entity.Property(m => m.RecebidoEm)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasIndex(m => m.Processado);
            });

            // =========================
            // PedidoEvento
            // =========================
            modelBuilder.Entity<PedidoEvento>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Pedido)
                      .WithMany(p => p.Eventos)
                      .HasForeignKey(e => e.PedidoId);

                entity.Property(e => e.Tipo)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Descricao)
                      .HasMaxLength(500);
            });


        }

    }
}
