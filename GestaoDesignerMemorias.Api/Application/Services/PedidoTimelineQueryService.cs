using GestaoDesignerMemorias.DTOs.Pedidos;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class PedidoTimelineQueryService
{
    private readonly AppDbContext _context;

    public PedidoTimelineQueryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PedidoTimelineDto>?> ObterAsync(Guid pedidoId)
    {
        var pedidoExiste = await _context.Pedidos
            .AnyAsync(p => p.Id == pedidoId);

        if (!pedidoExiste)
            return null;

        return await _context.PedidoEventos
            .Where(e => e.PedidoId == pedidoId)
            .OrderBy(e => e.CriadoEm)
            .Select(e => new PedidoTimelineDto
            {
                Tipo = e.Tipo,
                Descricao = e.Descricao,
                CriadoEm = e.CriadoEm
            })
            .ToListAsync();
    }
}
