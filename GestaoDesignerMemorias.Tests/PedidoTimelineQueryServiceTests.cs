using Application.Services;
using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerMemorias.Tests.Application.Services;

public class PedidoTimelineQueryServiceTests
{
    private readonly AppDbContext _context;
    private readonly PedidoTimelineQueryService _service;

    public PedidoTimelineQueryServiceTests()
    {
        _context = CriarContexto();
        _service = new PedidoTimelineQueryService(_context);
    }

    private static AppDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task DeveRetornarEventosOrdenadosPorData()
    {
        var pedidoId = Guid.NewGuid();

        _context.Pedidos.Add(new Pedido { Id = pedidoId, ClienteId = Guid.NewGuid(), TipoEvento = "teste" });

        _context.PedidoEventos.AddRange(
            new PedidoEvento
            {
                Id = Guid.NewGuid(),
                PedidoId = pedidoId,
                Tipo = "PedidoCriado",
                CriadoEm = DateTime.UtcNow.AddMinutes(-10)
            },
            new PedidoEvento
            {
                Id = Guid.NewGuid(),
                PedidoId = pedidoId,
                Tipo = "BriefingConcluido",
                CriadoEm = DateTime.UtcNow
            }
        );

        await _context.SaveChangesAsync();

        var timeline = await _service.ObterAsync(pedidoId);

        Assert.NotNull(timeline);
        Assert.Equal(2, timeline!.Count);
        Assert.Equal("PedidoCriado", timeline[0].Tipo);
        Assert.Equal("BriefingConcluido", timeline[1].Tipo);
    }
}
