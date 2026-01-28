using Application.Services;
using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerMemorias.Tests.Application.Services;

public class BriefingRespostaServiceTests
{
    private readonly AppDbContext _context;
    private readonly PedidoStatusService _pedidoStatusService;
    private readonly BriefingRespostaService _service;

    public BriefingRespostaServiceTests()
    {
        _context = CriarContextoEmMemoria();
        _pedidoStatusService = new PedidoStatusService(_context);
        _service = new BriefingRespostaService(_context, _pedidoStatusService);
    }

    private static AppDbContext CriarContextoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private async Task<(Pedido pedido, BriefingItem briefingItem)> CriarPedidoComBriefingItemAsync()
    {
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

        _context.Pedidos.Add(pedido);
        _context.BriefingItens.Add(briefingItem);
        await _context.SaveChangesAsync();

        return (pedido, briefingItem);
    }

    [Fact]
    public async Task RegistrarRespostaAsync_DeveSalvarRespostaNoBriefingItem()
    {
        // Arrange
        var (_, briefingItem) = await CriarPedidoComBriefingItemAsync();

        // Act
        var resultado = await _service.RegistrarRespostaAsync(
            briefingItem.Id,
            "Princesas"
        );

        // Assert
        Assert.True(resultado);

        var itemSalvo = await _context.BriefingItens
            .FirstAsync(b => b.Id == briefingItem.Id);

        Assert.Equal("Princesas", itemSalvo.Resposta);
    }

    [Fact]
    public async Task RegistrarRespostaAsync_DeveRetornarFalse_QuandoBriefingItemNaoExiste()
    {
        // Arrange
        var idInexistente = Guid.NewGuid();

        // Act
        var resultado = await _service.RegistrarRespostaAsync(
            idInexistente,
            "Qualquer resposta"
        );

        // Assert
        Assert.False(resultado);
    }

    // Método auxiliar opcional: limpar o contexto entre testes (caso precise em cenários mais complexos)
     [Fact] public void Dispose() => _context.Dispose();
}