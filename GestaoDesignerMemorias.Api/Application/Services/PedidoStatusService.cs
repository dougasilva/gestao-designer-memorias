using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;
public class PedidoStatusService
{
    private readonly AppDbContext _context;
    private readonly PedidoTimelineService _timelineService;
    private readonly PedidoTransicaoService _transicaoService;

    public PedidoStatusService(AppDbContext context, PedidoTimelineService timelineService, PedidoTransicaoService transicaoService)
    {
        _context = context;
        _timelineService = timelineService;
        _transicaoService = transicaoService;
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
            await _transicaoService.AlterarStatusAsync(
                pedido.Id,
                StatusPedido.OrcamentoSolicitado,
                "Briefing completo"
            );

            await _timelineService.RegistrarAsync(
                pedido.Id,
                "BriefingConcluido",
                "Todas as perguntas respondidas. Status alterado para OrcamentoSolicitado"
            );

            await _context.SaveChangesAsync();
        }

        return true;
    }
}
