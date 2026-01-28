using Application.Services;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class RegraPagamentoService
{
    private readonly AppDbContext _context;
    private readonly PagamentoStatusService _statusService;

    public RegraPagamentoService(
        AppDbContext context,
        PagamentoStatusService statusService)
    {
        _context = context;
        _statusService = statusService;
    }

    public async Task<bool> AplicarAsync(Guid pedidoId)
    {
        var pedido = await _context.Pedidos
            .FirstOrDefaultAsync(p => p.Id == pedidoId);

        if (pedido == null || pedido.ValorTotal <= 0)
            return false;

        var metade = pedido.ValorTotal / 2;

        switch (pedido.Marco)
        {
            case MarcoPedido.OrcamentoAprovado:
                pedido.ValorPago = Math.Max(pedido.ValorPago, metade);
                break;

            case MarcoPedido.Entregue:
                pedido.ValorPago = pedido.ValorTotal;
                break;
        }

        await _context.SaveChangesAsync();
        await _statusService.AvaliarAsync(pedido.Id);

        return true;
    }
}
