using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;

namespace Application.Services;
public class PedidoAutoService : IPedidoAutoService
{
    private readonly AppDbContext _context;
    private readonly BriefingInicializacaoService _briefingInicializacaoService;
    private readonly PedidoTimelineService _timelineService;

    public PedidoAutoService(AppDbContext context, BriefingInicializacaoService briefingInicializacaoService, PedidoTimelineService timelineService)
    {
        _context = context;
        _briefingInicializacaoService = briefingInicializacaoService;
        _timelineService = timelineService;
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
            TipoEvento = "aniversário", // MVP, ok
            Status = StatusPedido.Prospecao,
            StatusPagamento = StatusPagamento.Nenhum
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        await _briefingInicializacaoService
            .CriarBriefingInicialAsync(pedido.Id);

        await _timelineService.RegistrarAsync(
            pedido.Id,
            pedido.Status,
            pedido.Marco,
            "Pedido criado automaticamente via WhatsApp"
        );


        return pedido.Id;
    }
}
