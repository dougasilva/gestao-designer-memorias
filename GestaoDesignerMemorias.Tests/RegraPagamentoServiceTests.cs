using Application.Services;
using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerMemorias.Tests.Application.Services;

public class RegraPagamentoServiceTests
{
    private static AppDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task DeveCobrarMetadeNaAprovacao()
    {
        using var context = CriarContexto();

        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            ClienteId = Guid.NewGuid(),
            TipoEvento = "casamento",
            ValorTotal = 2000,
            ValorPago = 0,
            Marco = MarcoPedido.OrcamentoAprovado
        };

        context.Pedidos.Add(pedido);
        await context.SaveChangesAsync();

        var statusService = new PagamentoStatusService(context);
        var regraService = new RegraPagamentoService(context, statusService);

        await regraService.AplicarAsync(pedido.Id);

        var atualizado = await context.Pedidos.FirstAsync();
        Assert.Equal(1000, atualizado.ValorPago);
        Assert.Equal(StatusPagamento.SinalPago, atualizado.StatusPagamento);
    }

    [Fact]
    public async Task DeveQuitarPagamentoNaEntrega()
    {
        using var context = CriarContexto();

        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            ClienteId = Guid.NewGuid(),
            TipoEvento = "casamento",
            ValorTotal = 2000,
            ValorPago = 1000,
            Marco = MarcoPedido.Entregue
        };

        context.Pedidos.Add(pedido);
        await context.SaveChangesAsync();

        var statusService = new PagamentoStatusService(context);
        var regraService = new RegraPagamentoService(context, statusService);

        await regraService.AplicarAsync(pedido.Id);

        var atualizado = await context.Pedidos.FirstAsync();
        Assert.Equal(2000, atualizado.ValorPago);
        Assert.Equal(StatusPagamento.Pago, atualizado.StatusPagamento);
    }
}
