using Application.Services;
using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerMemorias.Tests.Application.Services;

public class PagamentoStatusServiceTests
{
    private readonly AppDbContext _context;
    private readonly PagamentoStatusService _service;

    public PagamentoStatusServiceTests()
    {
        _context = CriarContextoEmMemoria();
        _service = new PagamentoStatusService(_context);
    }

    private static AppDbContext CriarContextoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private async Task<Pedido> CriarPedidoAsync(decimal valorTotal, decimal valorPago)
    {
        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            ClienteId = Guid.NewGuid(),
            TipoEvento = "aniversário",
            ValorTotal = valorTotal,
            ValorPago = valorPago
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        return pedido;
    }

    private async Task AssertStatusPagamentoAsync(Guid pedidoId, StatusPagamento statusEsperado)
    {
        var pedidoAtualizado = await _context.Pedidos
            .FirstAsync(p => p.Id == pedidoId);

        Assert.Equal(statusEsperado, pedidoAtualizado.StatusPagamento);
    }

    [Fact]
    public async Task AvaliarAsync_DeveMarcarPendente_QuandoValorTotalDefinidoEValorPagoZero()
    {
        // Arrange
        var pedido = await CriarPedidoAsync(1000m, 0m);

        // Act
        await _service.AvaliarAsync(pedido.Id);

        // Assert
        await AssertStatusPagamentoAsync(pedido.Id, StatusPagamento.Pendente);
    }

    [Fact]
    public async Task AvaliarAsync_DeveMarcarSinalPago_QuandoValorPagoParcial()
    {
        // Arrange
        var pedido = await CriarPedidoAsync(1000m, 500m);

        // Act
        await _service.AvaliarAsync(pedido.Id);

        // Assert
        await AssertStatusPagamentoAsync(pedido.Id, StatusPagamento.SinalPago);
    }

    [Fact]
    public async Task AvaliarAsync_DeveMarcarPago_QuandoValorPagoIgualOuMaiorQueTotal()
    {
        // Arrange
        var pedido = await CriarPedidoAsync(1000m, 1000m);

        // Act
        await _service.AvaliarAsync(pedido.Id);

        // Assert
        await AssertStatusPagamentoAsync(pedido.Id, StatusPagamento.Pago);
    }

    [Fact]
    public async Task AvaliarAsync_DeveMarcarPago_QuandoValorPagoExcedeTotal()
    {
        // Arrange
        var pedido = await CriarPedidoAsync(1000m, 1500m);

        // Act
        await _service.AvaliarAsync(pedido.Id);

        // Assert
        await AssertStatusPagamentoAsync(pedido.Id, StatusPagamento.Pago);
    }

    // Opcional: teste de caso inválido (pedido inexistente)
    [Fact]
    public async Task AvaliarAsync_NaoDeveFalhar_QuandoPedidoNaoExiste()
    {
        // Act
        var exception = await Record.ExceptionAsync(() =>
            _service.AvaliarAsync(Guid.NewGuid()));

        // Assert
        Assert.Null(exception);  // ou verifique o comportamento desejado do serviço
    }
}