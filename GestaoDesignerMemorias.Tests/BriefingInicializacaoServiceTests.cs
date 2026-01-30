using Application.Services;
using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerMemorias.Tests.Application.Services;

public class BriefingInicializacaoServiceTests
{
    private readonly AppDbContext _context;
    private readonly BriefingInicializacaoService _service;

    public BriefingInicializacaoServiceTests()
    {
        _context = CriarContextoEmMemoria();
        _service = new BriefingInicializacaoService(_context);
    }

    private static AppDbContext CriarContextoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private async Task<Pedido> CriarPedidoAsync()
    {
        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            ClienteId = Guid.NewGuid(),
            TipoEvento = "casamento"
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        return pedido;
    }

    [Fact]
    public async Task CriarBriefingInicialAsync_DeveCriarBriefingParaPedido()
    {
        // Arrange
        var pedido = await CriarPedidoAsync();

        // Act
        await _service.CriarBriefingInicialAsync(pedido.Id);

        // Assert
        var itens = await _context.BriefingItens
            .Where(b => b.PedidoId == pedido.Id)
            .ToListAsync();

        Assert.Single(itens);
        Assert.Equal("Descreva o evento com suas próprias palavras", itens[0].Pergunta);
        Assert.Equal(BriefingItemType.Texto, itens[0].Tipo);
    }

    [Fact]
    public async Task CriarBriefingInicialAsync_NaoDeveDuplicarBriefing()
    {
        // Arrange
        var pedido = await CriarPedidoAsync();

        await _service.CriarBriefingInicialAsync(pedido.Id);

        // Act
        await _service.CriarBriefingInicialAsync(pedido.Id);

        // Assert
        var total = await _context.BriefingItens
            .CountAsync(b => b.PedidoId == pedido.Id);

        Assert.Equal(1, total);
    }
}
