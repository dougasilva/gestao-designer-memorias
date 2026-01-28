using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;

namespace Application.Services
{
    public class PedidoAutoService : IPedidoAutoService
    {
        private readonly AppDbContext _context;

        public PedidoAutoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid?> CriarPedidoSeAplicavelAsync(
            Guid clienteId,
            TipoIntencaoMensagem intencao)
        {
            if (intencao != TipoIntencaoMensagem.Orcamento &&
                intencao != TipoIntencaoMensagem.Pedido)
                return null;

            var pedido = new Pedido
            {
                Id = Guid.NewGuid(),
                ClienteId = clienteId,
                TipoEvento = "A definir",
                Status = StatusPedido.Prospecao,
                StatusPagamento = StatusPagamento.Nenhum
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return pedido.Id;
        }
    }

}