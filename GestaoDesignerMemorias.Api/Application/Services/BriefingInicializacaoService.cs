using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class BriefingInicializacaoService
{
    private readonly AppDbContext _context;

    public BriefingInicializacaoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task CriarBriefingInicialAsync(Guid pedidoId)
    {
        var existe = await _context.BriefingItens
            .AnyAsync(b => b.PedidoId == pedidoId);

        if (existe)
            return;

        var itensIniciais = new List<BriefingItem>
        {
            new()
            {
                Id = Guid.NewGuid(),
                PedidoId = pedidoId,
                Pergunta = "Descreva o evento com suas próprias palavras",
                Ordem = 1,
                Tipo = BriefingItemType.Texto
            }
        };

        _context.BriefingItens.AddRange(itensIniciais);
        await _context.SaveChangesAsync();
    }
}
