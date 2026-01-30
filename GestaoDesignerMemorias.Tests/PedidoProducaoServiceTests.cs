using Application.Services;
using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GestaoDesignerMemorias.Tests.Application.Services;

public class PedidoProducaoServiceTests
{
    private readonly AppDbContext _context;
    private readonly PedidoProducaoService _service;

    public PedidoProducaoServiceTests()
    {
        _context = CriarContextoEmMemoria();
        _service = new PedidoProducaoService(_context);
    }

    private static AppDbContext CriarContextoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task IniciarAsync_DeveMoverParaEmProducao_QuandoStatusForAprovado()
    {
        // Arrange
        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            ClienteId = Guid.NewGuid(),
            Status = StatusPedido.Aprovado,
            TipoEvento = "aniversario"
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _service.IniciarAsync(pedido.Id);

        // Assert
        Assert.True(resultado);

        var atualizado = await _context.Pedidos.FirstAsync();
        Assert.Equal(StatusPedido.EmProducao, atualizado.Status);
    }

    [Fact]
    public async Task IniciarAsync_DeveRetornarFalse_QuandoPedidoNaoExiste()
    {
        // Act
        var resultado = await _service.IniciarAsync(Guid.NewGuid());

        // Assert
        Assert.False(resultado);
    }

    [Fact]
    public async Task IniciarAsync_DeveRetornarFalse_QuandoStatusNaoForAprovado()
    {
        // Arrange
        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            Status = StatusPedido.OrcamentoSolicitado,
            TipoEvento = "aniversario"
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _service.IniciarAsync(pedido.Id);

        // Assert
        Assert.False(resultado);

        var atualizado = await _context.Pedidos.FirstAsync();
        Assert.Equal(StatusPedido.OrcamentoSolicitado, atualizado.Status);
    }
}
