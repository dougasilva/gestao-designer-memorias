using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;
public class PedidoStatusService
{
    private readonly AppDbContext _context;
    private readonly PedidoTimelineService _timelineService;

    public PedidoStatusService(AppDbContext context, PedidoTimelineService timelineService)
    {
        _context = context;
        _timelineService = timelineService;
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

            await _timelineService.RegistrarAsync(
                pedido.Id,
                pedido.Status,
                pedido.Marco,
                "Briefing completo — orçamento solicitado"
            );

            await _context.SaveChangesAsync();
        }

        return true;
    }
}
