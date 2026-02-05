using Application.Services;
using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using GestaoDesignerMemorias.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace GestaoDesignerMemorias.Tests.Application.Services;

public class BriefingRespostaServiceTests : IDisposable
{
    private readonly AppDbContext _testContext;
    private readonly BriefingRespostaService _service;
    private readonly Mock<PedidoStatusService> _mockPedidoStatusService;

    public BriefingRespostaServiceTests()
    {
        _testContext = InMemoryDbContextFactory.Create();

        _mockPedidoStatusService = new Mock<PedidoStatusService>();
        _mockPedidoStatusService
            .Setup(s => s.AvaliarStatusAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        var respostaContext = InMemoryDbContextFactory.Create();

        _service = new BriefingRespostaService(
            respostaContext,
            _mockPedidoStatusService.Object
        );
    }

    public void Dispose()
    {
        _testContext.Dispose();
    }

    private async Task<BriefingItem> CriarPedidoComBriefingItemAsync()
    {
        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            ClienteId = Guid.NewGuid(),
            TipoEvento = "aniversário",
            Status = StatusPedido.Prospecao,
            BriefingItens = new List<BriefingItem>()
        };

        var briefingItem = new BriefingItem
        {
            Id = Guid.NewGuid(),
            PedidoId = pedido.Id,
            Pergunta = "Tema",
            Tipo = BriefingItemType.Texto,
            Resposta = null,
            Ordem = 1  // required
        };

        pedido.BriefingItens.Add(briefingItem);

        _testContext.Pedidos.Add(pedido);
        await _testContext.SaveChangesAsync();

        return briefingItem;
    }

    [Fact]
    public async Task RegistrarRespostaAsync_DeveSalvarRespostaNoBriefingItem()
    {
        var briefingItem = await CriarPedidoComBriefingItemAsync();

        var resultado = await _service.RegistrarRespostaAsync(
            briefingItem.Id,
            "Princesas"
        );

        Assert.True(resultado);

        var itemSalvo = await _testContext.BriefingItens
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == briefingItem.Id);

        Assert.NotNull(itemSalvo);
        Assert.Equal("Princesas", itemSalvo.Resposta);

        _mockPedidoStatusService.Verify(
            s => s.AvaliarStatusAsync(briefingItem.PedidoId),
            Times.Once()
        );
    }

    [Fact]
    public async Task RegistrarRespostaAsync_DeveRetornarFalse_QuandoBriefingItemNaoExiste()
    {
        var resultado = await _service.RegistrarRespostaAsync(
            Guid.NewGuid(),
            "Qualquer resposta"
        );

        Assert.False(resultado);

        _mockPedidoStatusService.Verify(
            s => s.AvaliarStatusAsync(It.IsAny<Guid>()),
            Times.Never()
        );
    }
}