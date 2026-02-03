using Application.Services;
using GestaoDesignerMemorias.Api.DTOs.Briefing;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/briefing")]
public class BriefingController : ControllerBase
{
    private readonly BriefingRespostaService _service;

    public BriefingController(BriefingRespostaService service)
    {
        _service = service;
    }

    [HttpPost("{briefingItemId:guid}/resposta")]
    public async Task<IActionResult> Responder(Guid briefingItemId, [FromBody] BriefingRespostaDto dto)
    {
        var sucesso = await _service.RegistrarRespostaAsync(
            briefingItemId,
            dto.Resposta
        );

        if (!sucesso)
            return NotFound();

        return Ok();
    }
}
