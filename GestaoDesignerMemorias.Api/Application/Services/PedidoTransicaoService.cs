using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class PedidoTransicaoService
{
    private readonly AppDbContext _context;
    private readonly PedidoTimelineService _timelineService;

    public PedidoTransicaoService(
        AppDbContext context,
        PedidoTimelineService timelineService)
    {
        _context = context;
        _timelineService = timelineService;
    }

    public async Task<bool> AlterarStatusAsync(
        Guid pedidoId,
        StatusPedido novoStatus,
        string motivo)
    {
        var pedido = await _context.Pedidos
            .FirstOrDefaultAsync(p => p.Id == pedidoId);

        if (pedido == null)
            return false;

        if (pedido.Status == novoStatus)
            return true;

        pedido.Status = novoStatus;

        await _context.SaveChangesAsync();

        await _timelineService.RegistrarAsync(
            pedido.Id,
            "StatusAlterado",
            $"Status alterado para {novoStatus}. Motivo: {motivo}"
        );

        return true;
    }

    public async Task<bool> AlterarMarcoAsync(
        Guid pedidoId,
        MarcoPedido novoMarco,
        string motivo)
    {
        var pedido = await _context.Pedidos
            .FirstOrDefaultAsync(p => p.Id == pedidoId);

        if (pedido == null)
            return false;

        if (pedido.Marco == novoMarco)
            return true;

        pedido.Marco = novoMarco;

        await _context.SaveChangesAsync();

        await _timelineService.RegistrarAsync(
            pedido.Id,
            "MarcoAlterado",
            $"Marco alterado para {novoMarco}. Motivo: {motivo}"
        );

        return true;
    }
}
