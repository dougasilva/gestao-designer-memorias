using Application.Services;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GestaoDesignerMemorias.Tests.Application.Services;

public class PedidoAutoServiceTests
{
    private readonly AppDbContext _context;
    private readonly PedidoAutoService _service;
    private readonly PedidoTimelineService _pedidoTimelineService;

    public PedidoAutoServiceTests()
    {
        _context = CriarContextoEmMemoria();
        var briefingInicializacaoService = new BriefingInicializacaoService(_context);
        _pedidoTimelineService = new PedidoTimelineService(_context);
        _service = new PedidoAutoService(_context, briefingInicializacaoService, _pedidoTimelineService);
        
    }

    private static AppDbContext CriarContextoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CriarPedidoSeAplicavelAsync_DeveCriarPedidoEBriefingInicial()
    {
        // Arrange
        var clienteId = Guid.NewGuid();

        // Act
        var pedidoId = await _service.CriarPedidoSeAplicavelAsync(
            clienteId,
            TipoIntencaoMensagem.Orcamento);

        // Assert
        Assert.NotNull(pedidoId);

        var pedido = await _context.Pedidos
            .Include(p => p.BriefingItens)
            .FirstOrDefaultAsync(p => p.Id == pedidoId);

        Assert.NotNull(pedido);
        Assert.NotEmpty(pedido!.BriefingItens);

        // Template de aniversário alguma pergunta
        Assert.True(pedido.BriefingItens.Any());
    }

    [Fact]
    public async Task CriarPedidoSeAplicavelAsync_NaoDeveCriarPedido_QuandoIntencaoInvalida()
    {
        // Arrange
        var clienteId = Guid.NewGuid();

        // Act
        var pedidoId = await _service.CriarPedidoSeAplicavelAsync(
            clienteId,
            TipoIntencaoMensagem.Outros);

        // Assert
        Assert.Null(pedidoId);
        Assert.Empty(_context.Pedidos);
        Assert.Empty(_context.BriefingItens);
    }
}
