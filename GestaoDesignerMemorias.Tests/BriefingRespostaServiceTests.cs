using Application.Services;
using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerMemorias.Tests.Application.Services;

public class BriefingRespostaServiceTests
{
    private static AppDbContext CriarContextoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task RegistrarRespostaAsync_DeveSalvarRespostaNoBriefingItem()
    {
        // Arrange
        using var context = CriarContextoEmMemoria();

        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            ClienteId = Guid.NewGuid(),
            TipoEvento = "aniversário"
        };

        var briefingItem = new BriefingItem
        {
            Id = Guid.NewGuid(),
            PedidoId = pedido.Id,
            Pergunta = "Tema do evento",
            Tipo = BriefingItemType.Texto
        };

        context.Pedidos.Add(pedido);
        context.BriefingItens.Add(briefingItem);
        await context.SaveChangesAsync();

        var service = new BriefingRespostaService(context);

        // Act
        var resultado = await service.RegistrarRespostaAsync(
            briefingItem.Id,
            "Princesas"
        );

        // Assert
        Assert.True(resultado);

        var itemSalvo = await context.BriefingItens
            .FirstAsync(b => b.Id == briefingItem.Id);

        Assert.Equal("Princesas", itemSalvo.Resposta);
    }

    [Fact]
    public async Task RegistrarRespostaAsync_DeveRetornarFalse_SeBriefingItemNaoExistir()
    {
        // Arrange
        using var context = CriarContextoEmMemoria();
        var service = new BriefingRespostaService(context);

        // Act
        var resultado = await service.RegistrarRespostaAsync(
            Guid.NewGuid(),
            "Qualquer coisa"
        );

        // Assert
        Assert.False(resultado);
    }
}
