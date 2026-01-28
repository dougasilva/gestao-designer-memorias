using Application.Services;
using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.DTOs.Webhooks;
using GestaoDesignerMemorias.Infrastructure;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerMemorias.Api.Controllers;

[ApiController]
[Route("api/webhook")]
public class WebhookController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMessageClassifier _messageClassifier;
    private readonly IPedidoAutoService _pedidoAutoService;
    private readonly ILogger<WebhookController> _logger;

    public WebhookController(
        AppDbContext context,
        IMessageClassifier messageClassifier,
        IPedidoAutoService pedidoAutoService,
        ILogger<WebhookController> logger)
    {
        _context = context;
        _messageClassifier = messageClassifier;
        _pedidoAutoService = pedidoAutoService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> ReceberMensagem([FromBody] WebhookDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Telefone) ||
            string.IsNullOrWhiteSpace(dto.Mensagem))
        {
            return BadRequest("Payload inválido");
        }

        // 1️ - Busca ou cria cliente
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.Telefone == dto.Telefone);

        if (cliente == null)
        {
            cliente = new Cliente
            {
                Id = Guid.NewGuid(),
                Telefone = dto.Telefone,
                Nome = dto.Nome ?? "Contato WhatsApp"
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        // 2️ - Classifica intenção
        var intencao = _messageClassifier.Classificar(dto.Mensagem);

        // 3️ - Cria pedido se aplicável
        await _pedidoAutoService.CriarPedidoSeAplicavelAsync(
            cliente.Id,
            intencao
        );

        return Ok();
    }
}
