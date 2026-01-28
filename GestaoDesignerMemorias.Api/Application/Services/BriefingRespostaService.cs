using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;
public class BriefingRespostaService
{
    private readonly AppDbContext _context;
    private readonly PedidoStatusService _pedidoStatusService;

    public BriefingRespostaService(AppDbContext context, PedidoStatusService pedidoStatusService)
    {
        _context = context;
        _pedidoStatusService = pedidoStatusService;
    }

    public async Task<bool> RegistrarRespostaAsync(
        Guid briefingItemId,
        string resposta)
    {
        var item = await _context.BriefingItens
            .FirstOrDefaultAsync(b => b.Id == briefingItemId);

        if (item == null)
            return false;

        item.Resposta = resposta;

        await _context.SaveChangesAsync();

        await _pedidoStatusService.AvaliarStatusAsync(item.PedidoId);

        return true;
    }
}
