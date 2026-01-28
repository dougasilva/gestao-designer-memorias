using GestaoDesignerMemorias.Domain.Briefing;
using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;

namespace Application.Services;
public class PedidoAutoService : IPedidoAutoService
{
    private readonly AppDbContext _context;

    public PedidoAutoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid?> CriarPedidoSeAplicavelAsync(
        Guid clienteId,
        TipoIntencaoMensagem intencao)
    {
        if (intencao != TipoIntencaoMensagem.Orcamento &&
            intencao != TipoIntencaoMensagem.Pedido)
            return null;

        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            ClienteId = clienteId,
            TipoEvento = "aniversário", // primeira versão, será teste - fixo
            Status = StatusPedido.Prospecao,
            StatusPagamento = StatusPagamento.Nenhum
        };

        _context.Pedidos.Add(pedido);

        // - BRF-03
        CriarBriefingInicial(pedido);

        await _context.SaveChangesAsync();

        return pedido.Id;
    }

    private void CriarBriefingInicial(Pedido pedido)
    {
        var template =
            BriefingTemplateFactory.Criar(pedido.TipoEvento);

        foreach (var item in template)
        {
            pedido.BriefingItems.Add(new BriefingItem
            {
                Id = Guid.NewGuid(),
                PedidoId = pedido.Id,
                Pergunta = item.Pergunta,
                Tipo = item.Tipo,
                Opcoes = item.Opcoes
            });
        }
    }

}