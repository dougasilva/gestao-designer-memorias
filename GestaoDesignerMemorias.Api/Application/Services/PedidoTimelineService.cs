using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Infrastructure.Data;

public class PedidoTimelineService
{
    private readonly AppDbContext _context;

    public PedidoTimelineService(AppDbContext context)
    {
        _context = context;
    }

    public async Task RegistrarAsync(Guid pedidoId, string tipo, string? descricao = null)
    {
        var evento = new PedidoEvento
        {
            Id = Guid.NewGuid(),
            PedidoId = pedidoId,
            Tipo = tipo,
            Descricao = descricao
        };

        _context.PedidoEventos.Add(evento);
        await _context.SaveChangesAsync();
    }
}
