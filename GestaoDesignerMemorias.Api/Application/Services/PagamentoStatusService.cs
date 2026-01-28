using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;
public class PagamentoStatusService
{
    private readonly AppDbContext _context;

    public PagamentoStatusService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AvaliarAsync(Guid pedidoId)
    {
        var pedido = await _context.Pedidos
            .FirstOrDefaultAsync(p => p.Id == pedidoId);

        if (pedido == null)
            return false;

        if (pedido.ValorTotal <= 0)
        {
            pedido.StatusPagamento = StatusPagamento.Nenhum;
        }
        else if (pedido.ValorPago <= 0)
        {
            pedido.StatusPagamento = StatusPagamento.Pendente;
        }
        else if (pedido.ValorPago < pedido.ValorTotal)
        {
            pedido.StatusPagamento = StatusPagamento.SinalPago;
        }
        else
        {
            pedido.StatusPagamento = StatusPagamento.Pago;
        }

        await _context.SaveChangesAsync();
        return true;
    }
}
