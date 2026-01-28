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

    [HttpPost("responder")]
    public async Task<IActionResult> Responder([FromBody] BriefingRespostaDto dto)
    {
        var sucesso = await _service.RegistrarRespostaAsync(
            dto.BriefingItemId,
            dto.Resposta
        );

        if (!sucesso)
            return NotFound();

        return Ok();
    }
}
