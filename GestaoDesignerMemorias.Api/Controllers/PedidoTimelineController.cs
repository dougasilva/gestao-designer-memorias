using Application.Services;
using GestaoDesignerMemorias.DTOs.Pedidos;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDesignerMemorias.Api.Controllers;

[ApiController]
[Route("api/pedidos")]
public class PedidoTimelineController : ControllerBase
{
    private readonly PedidoTimelineQueryService _service;

    public PedidoTimelineController(PedidoTimelineQueryService service)
    {
        _service = service;
    }

    [HttpGet("{pedidoId:guid}/timeline")]
    public async Task<ActionResult<List<PedidoTimelineDto>>> GetTimeline(Guid pedidoId)
    {
        var timeline = await _service.ObterAsync(pedidoId);

        if (timeline == null)
            return NotFound();

        return Ok(timeline);
    }
}
