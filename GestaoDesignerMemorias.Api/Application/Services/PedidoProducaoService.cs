using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class PedidoProducaoService
{
    private readonly AppDbContext _context;

    public PedidoProducaoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IniciarAsync(Guid pedidoId)
    {
        var pedido = await _context.Pedidos
            .FirstOrDefaultAsync(p => p.Id == pedidoId);

        if (pedido == null)
            return false;

        if (pedido.Status != StatusPedido.Aprovado)
            return false;

        pedido.Status = StatusPedido.EmProducao;
        await _context.SaveChangesAsync();

        return true;
    }
}
