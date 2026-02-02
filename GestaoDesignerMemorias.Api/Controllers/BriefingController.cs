using Application.Services;
using GestaoDesignerMemorias.Api.DTOs.Briefing;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerMemorias.Api.Controllers;

[ApiController]
[Route("api/briefing")]
public class BriefingController : ControllerBase
{
    private readonly BriefingRespostaService _service;
    private readonly AppDbContext _context;

    public BriefingController(BriefingRespostaService service, AppDbContext context)
    {
        _service = service;
        _context = context;
    }

    // POST: api/briefing/{briefingItemId}/resposta
    [HttpPost("{briefingItemId:guid}/resposta")]
    public async Task<IActionResult> Responder(Guid briefingItemId, [FromBody] BriefingRespostaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Resposta))
            return BadRequest("Resposta não pode ser vazia.");

        var sucesso = await _service.RegistrarRespostaAsync(
            briefingItemId,
            dto.Resposta
        );

        if (!sucesso)
            return NotFound();

        return Ok();
    }

    // GET: api/pedidos/{pedidoId}/briefing
    [HttpGet("{pedidoId:guid}/briefing")]
    public async Task<ActionResult<IEnumerable<BriefingItemResponseDto>>> GetBriefing(Guid pedidoId)
    {
        var pedidoExiste = await _context.Pedidos
            .AnyAsync(p => p.Id == pedidoId);

        if (!pedidoExiste)
            return NotFound("Pedido não encontrado.");

        var briefing = await _context.BriefingItens
            .Where(b => b.PedidoId == pedidoId)
            .OrderBy(b => b.Ordem)
            .Select(b => new BriefingItemResponseDto
            {
                Id = b.Id,
                Pergunta = b.Pergunta,
                Resposta = b.Resposta,
                Ordem = b.Ordem,
                Tipo = b.Tipo,
                Opcoes = b.Opcoes
            })
            .ToListAsync();

        return Ok(briefing);
    }

}
