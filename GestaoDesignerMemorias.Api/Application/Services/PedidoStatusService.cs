using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;
public class PedidoStatusService
{
    private readonly AppDbContext _context;

    public PedidoStatusService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AvaliarStatusAsync(Guid pedidoId)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.BriefingItens)
            .FirstOrDefaultAsync(p => p.Id == pedidoId);

        if (pedido == null || !pedido.BriefingItens.Any())
            return false;

        var todosRespondidos =
            pedido.BriefingItens.All(b => !string.IsNullOrWhiteSpace(b.Resposta));

        if (todosRespondidos && pedido.Status != StatusPedido.OrcamentoSolicitado)
        {
            pedido.Status = StatusPedido.OrcamentoSolicitado;
            await _context.SaveChangesAsync();
        }

        return true;
    }
}
