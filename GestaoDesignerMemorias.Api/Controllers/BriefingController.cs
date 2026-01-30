using Application.Services;
using GestaoDesignerMemorias.Api.DTOs.Briefing;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDesignerMemorias.Api.Controllers;

[ApiController]
[Route("api/briefing")]
public class BriefingController : ControllerBase
{
    private readonly BriefingRespostaService _service;

    public BriefingController(BriefingRespostaService service)
    {
        _service = service;
    }

    // POST: api/briefing/{briefingItemId}/resposta
    [HttpPost("{briefingItemId:guid}/resposta")]
    public async Task<IActionResult> Responder(
        Guid briefingItemId,
        [FromBody] BriefingRespostaDto dto)
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
}
