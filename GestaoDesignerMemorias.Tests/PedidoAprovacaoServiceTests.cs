using Application.Services;
using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GestaoDesignerMemorias.Tests.Application.Services;

public class PedidoAprovacaoServiceTests
{
    private readonly AppDbContext _context;
    private readonly PedidoAprovacaoService _service;

    public PedidoAprovacaoServiceTests()
    {
        _context = CriarContextoEmMemoria();
        _service = new PedidoAprovacaoService(_context);
    }

    private static AppDbContext CriarContextoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task AprovarAsync_DeveAprovarPedido_QuandoStatusForOrcamentoSolicitado()
    {
        // Arrange
        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            ClienteId = Guid.NewGuid(),
            Status = StatusPedido.OrcamentoSolicitado,
            TipoEvento = "aniversário",
            ValorTotal = 0
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _service.AprovarAsync(
            pedido.Id,
            1500m);

        // Assert
        Assert.True(resultado);

        var pedidoAtualizado = await _context.Pedidos
            .FirstAsync(p => p.Id == pedido.Id);

        Assert.Equal(StatusPedido.Aprovado, pedidoAtualizado.Status);
        Assert.Equal(1500m, pedidoAtualizado.ValorTotal);
    }

    [Fact]
    public async Task AprovarAsync_DeveRetornarFalse_QuandoPedidoNaoExiste()
    {
        // Arrange
        var pedidoIdInexistente = Guid.NewGuid();

        // Act
        var resultado = await _service.AprovarAsync(
            pedidoIdInexistente,
            1000m);

        // Assert
        Assert.False(resultado);
    }

    [Fact]
    public async Task AprovarAsync_DeveRetornarFalse_QuandoStatusNaoForOrcamentoSolicitado()
    {
        // Arrange
        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            ClienteId = Guid.NewGuid(),
            Status = StatusPedido.Prospecao,
            TipoEvento = "aniversário"
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _service.AprovarAsync(
            pedido.Id,
            1200m);

        // Assert
        Assert.False(resultado);

        var pedidoAtualizado = await _context.Pedidos
            .FirstAsync(p => p.Id == pedido.Id);

        Assert.Equal(StatusPedido.Prospecao, pedidoAtualizado.Status);
        Assert.Equal(0, pedidoAtualizado.ValorTotal);
    }
}
