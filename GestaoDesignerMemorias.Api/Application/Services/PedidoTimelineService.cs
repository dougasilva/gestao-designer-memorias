using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;

namespace Application.Services;

public class PedidoTimelineService
{
    private readonly AppDbContext _context;

    public PedidoTimelineService(AppDbContext context)
    {
        _context = context;
    }

    public async Task RegistrarAsync(
        Guid pedidoId,
        StatusPedido status,
        MarcoPedido marco,
        string evento)
    {
        var timeline = new PedidoTimeline
        {
            Id = Guid.NewGuid(),
            PedidoId = pedidoId,
            Status = status,
            Marco = marco,
            Evento = evento
        };

        _context.PedidoTimelines.Add(timeline);
        await _context.SaveChangesAsync();
    }
}
