using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;
public class BriefingRespostaService
{
    private readonly AppDbContext _context;

    public BriefingRespostaService(AppDbContext context)
    {
        _context = context;
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
        return true;
    }
}
