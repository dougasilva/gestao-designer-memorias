using Application.Services;
using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerMemorias.Tests.Application.Services;

public class PedidoAutoServiceTests
{
    private static AppDbContext CriarContextoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static BriefingInicializacaoService CriarBriefingIni(AppDbContext context)
    {
        return new BriefingInicializacaoService(context);
    }

    [Fact]
    public async Task CriarPedidoSeAplicavelAsync_DeveCriarBriefingAutomaticamente()
    {
        // Arrange
        using var context = CriarContextoEmMemoria();

        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            Nome = "Cliente Teste",
            Telefone = "11999999999"
        };

        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var briefingInicial = CriarBriefingIni(context);

        var service = new PedidoAutoService(context, briefingInicial);

        // Act
        var pedidoId = await service.CriarPedidoSeAplicavelAsync(
            cliente.Id,
            TipoIntencaoMensagem.Pedido
        );

        // Assert
        Assert.NotNull(pedidoId);

        var pedido = await context.Pedidos
            .Include(p => p.BriefingItens)
            .FirstOrDefaultAsync(p => p.Id == pedidoId);

        Assert.NotNull(pedido);
        Assert.NotEmpty(pedido!.BriefingItens);

        // sanity check: pelo menos uma pergunta esperada
        Assert.Contains(
            pedido.BriefingItens,
            b => b.Pergunta == "Descreva o evento com suas próprias palavras"
        );
    }

    [Fact]
    public async Task CriarPedidoSeAplicavelAsync_NaoDeveCriarPedido_ParaIntencaoIrrelevante()
    {
        // Arrange
        using var context = CriarContextoEmMemoria();

        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            Nome = "Cliente Teste",
            Telefone = "11888888888"
        };

        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var briefingInicial = CriarBriefingIni(context);

        var service = new PedidoAutoService(context, briefingInicial);

        // Act
        var pedidoId = await service.CriarPedidoSeAplicavelAsync(
            cliente.Id,
            TipoIntencaoMensagem.Duvida
        );

        // Assert
        Assert.Null(pedidoId);
        Assert.Empty(context.Pedidos);
        Assert.Empty(context.BriefingItens);
    }
}
