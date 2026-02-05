using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class PedidoTransicaoService
{
    private readonly AppDbContext _context;

    public PedidoTransicaoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExecutarAsync(
        Guid pedidoId,
        AcaoPedido acao,
        string? descricao = null)
    {
        var pedido = await _context.Pedidos
                     .Include(p => p.Eventos)
                     .FirstOrDefaultAsync(p => p.Id == pedidoId);

        if (pedido == null)
            return false;

        switch (acao)
        {
            case AcaoPedido.BriefingConcluido:
                if (pedido.Status != StatusPedido.BriefingEmAndamento &&
                    pedido.Status != StatusPedido.Prospecao)
                    return false;

                pedido.Status = StatusPedido.OrcamentoSolicitado;
                RegistrarEvento(pedido, "BriefingConcluido", descricao);
                break;

            case AcaoPedido.OrcamentoAprovado:
                if (pedido.Status != StatusPedido.OrcamentoSolicitado)
                    return false;

                pedido.Status = StatusPedido.Aprovado;
                pedido.Marco = MarcoPedido.OrcamentoAprovado;
                RegistrarEvento(pedido, "OrcamentoAprovado", descricao);
                break;

            case AcaoPedido.IniciarProducao:
                if (pedido.Status != StatusPedido.Aprovado)
                    return false;

                pedido.Status = StatusPedido.EmProducao;
                RegistrarEvento(pedido, "ProducaoIniciada", descricao);
                break;

            case AcaoPedido.MarcarEntregue:
                if (pedido.Status != StatusPedido.EmProducao)
                    return false;

                pedido.Status = StatusPedido.Entregue;
                pedido.Marco = MarcoPedido.Entregue;
                RegistrarEvento(pedido, "PedidoEntregue", descricao);
                break;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    private void RegistrarEvento(Pedido pedido, string tipo, string? descricao)
    {
        pedido.Eventos.Add(new PedidoEvento
        {
            Id = Guid.NewGuid(),
            PedidoId = pedido.Id,
            Tipo = tipo,
            Descricao = descricao
        });
    }
}
