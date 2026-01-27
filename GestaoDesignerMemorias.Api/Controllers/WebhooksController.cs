using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.DTOs.Webhooks;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerMemorias.Controllers;

[ApiController]
[Route("api/webhooks")]
public class WebhooksController : ControllerBase
{
    private readonly AppDbContext _context;

    public WebhooksController(AppDbContext context)
    {
        _context = context;
    }

    // POST: api/webhooks/whatsapp
    [HttpPost("whatsapp")]
    public async Task<IActionResult> ReceiveWhatsAppMessage(WhatsAppWebhookDto dto)
    {
        // 1. Log da mensagem (sempre)
        var log = new MensagemWebhook
        {
            Id = Guid.NewGuid(),
            TelefoneOrigem = dto.Telefone,
            Conteudo = dto.Mensagem,
            RecebidoEm = DateTime.UtcNow
        };

        _context.MensagensWebhook.Add(log);

        // 2. Cliente: busca ou cria
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.Telefone == dto.Telefone);

        if (cliente == null)
        {
            cliente = new Cliente
            {
                Id = Guid.NewGuid(),
                Nome = dto.Nome ?? "Contato WhatsApp",
                Telefone = dto.Telefone
            };

            _context.Clientes.Add(cliente);
        }

        // 3. Regra simples por palavra-chave
        var mensagemLower = dto.Mensagem.ToLower();

        if (mensagemLower.Contains("orçamento") || mensagemLower.Contains("orcamento"))
        {
            var pedido = new Pedido
            {
                Id = Guid.NewGuid(),
                ClienteId = cliente.Id,
                TipoEvento = "A definir",
                ValorTotal = 0,
                Status = StatusPedido.OrcamentoSolicitado,
                StatusPagamento = StatusPagamento.Nenhum,
                DataCriacao = DateTime.UtcNow
            };

            _context.Pedidos.Add(pedido);
        }

        await _context.SaveChangesAsync();

        return Ok(new { status = "recebido" });
    }
}
