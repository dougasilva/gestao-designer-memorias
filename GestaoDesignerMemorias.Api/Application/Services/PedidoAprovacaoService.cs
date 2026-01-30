using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class PedidoAprovacaoService
{
    private readonly AppDbContext _context;

    public PedidoAprovacaoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AprovarAsync(Guid pedidoId, decimal valorTotal)
    {
        var pedido = await _context.Pedidos
            .FirstOrDefaultAsync(p => p.Id == pedidoId);

        if (pedido == null)
            return false;

        if (pedido.Status != StatusPedido.OrcamentoSolicitado)
            return false;

        pedido.Status = StatusPedido.Aprovado;
        pedido.ValorTotal = valorTotal;

        await _context.SaveChangesAsync();
        return true;
    }
}
