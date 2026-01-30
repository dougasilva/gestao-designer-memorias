using Application.Services;
using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GestaoDesignerMemorias.Tests.Application.Services;

public class PedidoEntregaServiceTests
{
    private readonly AppDbContext _context;
    private readonly PedidoEntregaService _service;

    public PedidoEntregaServiceTests()
    {
        _context = CriarContextoEmMemoria();
        _service = new PedidoEntregaService(_context);
    }

    private static AppDbContext CriarContextoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task EntregarAsync_DeveMoverParaEntregue_QuandoStatusForEmProducao()
    {
        // Arrange
        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            ClienteId = Guid.NewGuid(),
            Status = StatusPedido.EmProducao,
            TipoEvento = "Casamento"
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _service.EntregarAsync(pedido.Id);

        // Assert
        Assert.True(resultado);

        var atualizado = await _context.Pedidos.FirstAsync();
        Assert.Equal(StatusPedido.Entregue, atualizado.Status);
    }

    [Fact]
    public async Task EntregarAsync_DeveRetornarFalse_QuandoPedidoNaoExiste()
    {
        // Act
        var resultado = await _service.EntregarAsync(Guid.NewGuid());

        // Assert
        Assert.False(resultado);
    }

    [Fact]
    public async Task EntregarAsync_DeveRetornarFalse_QuandoStatusNaoForEmProducao()
    {
        // Arrange
        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            Status = StatusPedido.Aprovado,
            TipoEvento = "Casamento"
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _service.EntregarAsync(pedido.Id);

        // Assert
        Assert.False(resultado);

        var atualizado = await _context.Pedidos.FirstAsync();
        Assert.Equal(StatusPedido.Aprovado, atualizado.Status);
    }
}
