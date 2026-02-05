using Application.Services;
using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerMemorias.Tests.Application.Services;

public class PedidoStatusServiceTests
{
    private static AppDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static PedidoTimelineService CriarPedidoTimelineService(AppDbContext context)
    {
        return new PedidoTimelineService(context);
    }

    private static PedidoTransicaoService CriarPedidoTransicaoService(AppDbContext context)
    {
        var timelineService = CriarPedidoTimelineService(context);
        return new PedidoTransicaoService(context);
    }

    [Fact]
    public async Task AvaliarStatusAsync_DeveMudarParaEmOrcamento_QuandoTodosRespondidos()
    {
        using var context = CriarContexto();

        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            ClienteId = Guid.NewGuid(),
            TipoEvento = "aniversário",
            Status = StatusPedido.Prospecao
        };

        var item1 = new BriefingItem
        {
            Id = Guid.NewGuid(),
            PedidoId = pedido.Id,
            Pergunta = "Tema",
            Tipo = BriefingItemType.Texto,
            Resposta = "Princesas"
        };

        var item2 = new BriefingItem
        {
            Id = Guid.NewGuid(),
            PedidoId = pedido.Id,
            Pergunta = "Data",
            Tipo = BriefingItemType.Data,
            Resposta = "2026-03-10"
        };

        context.Pedidos.Add(pedido);
        context.BriefingItens.AddRange(item1, item2);
        await context.SaveChangesAsync();

        var service = new PedidoStatusService(context, CriarPedidoTimelineService(context), CriarPedidoTransicaoService(context));

        // Act
        await service.AvaliarStatusAsync(pedido.Id);

        // Assert
        var atualizado = await context.Pedidos.FirstAsync();
        Assert.Equal(StatusPedido.OrcamentoSolicitado, atualizado.Status);
    }

    [Fact]
    public async Task AvaliarStatusAsync_NaoDeveAlterarStatus_SeExistirRespostaVazia()
    {
        using var context = CriarContexto();

        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            ClienteId = Guid.NewGuid(),
            TipoEvento = "aniversário",
            Status = StatusPedido.Prospecao
        };

        var item = new BriefingItem
        {
            Id = Guid.NewGuid(),
            PedidoId = pedido.Id,
            Pergunta = "Tema",
            Tipo = BriefingItemType.Texto,
            Resposta = null
        };

        context.Pedidos.Add(pedido);
        context.BriefingItens.Add(item);
        await context.SaveChangesAsync();

        var service = new PedidoStatusService(context, CriarPedidoTimelineService(context), CriarPedidoTransicaoService(context));

        // Act
        await service.AvaliarStatusAsync(pedido.Id);

        // Assert
        var atualizado = await context.Pedidos.FirstAsync();
        Assert.Equal(StatusPedido.Prospecao, atualizado.Status);
    }
}
