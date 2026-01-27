using Application.Services;
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
    private readonly IMessageClassifier _classifier;
    private readonly ILogger<WebhooksController> _logger;

    public WebhooksController(
        AppDbContext context,
        IMessageClassifier classifier,
        ILogger<WebhooksController> logger)
    {
        _context = context;
        _classifier = classifier;
        _logger = logger;
    }

    [HttpPost("whatsapp")]
    public async Task<IActionResult> ReceiveWhatsapp([FromBody] WhatsAppWebhookDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Mensagem))
            return BadRequest("Mensagem inválida");

        // 1️⃣ Classificar mensagem
        var category = _classifier.Classify(dto.Mensagem);

        _logger.LogInformation(
            "Webhook WhatsApp | Telefone={Telefone} | Categoria={Categoria}",
            dto.Telefone,
            category);

        // 2️⃣ Log da mensagem (auditável)
        var log = new MensagemWebhook
        {
            Id = Guid.NewGuid(),
            TelefoneOrigem = dto.Telefone,
            Conteudo = dto.Mensagem,
            RecebidoEm = DateTime.UtcNow
        };
        _context.MensagensWebhook.Add(log);

        // 3️⃣ Buscar ou criar cliente
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.Telefone == dto.Telefone);

        if (cliente == null)
        {
            cliente = new Cliente
            {
                Id = Guid.NewGuid(),
                Nome = string.IsNullOrWhiteSpace(dto.Nome)
                    ? "Contato WhatsApp"
                    : dto.Nome,
                Telefone = dto.Telefone
            };
            _context.Clientes.Add(cliente);
        }

        // 4️⃣ Decisão por categoria
        switch (category)
        {
            case MessageCategory.Orcamento:
                CriarPedidoSeNaoExistir(cliente.Id, StatusPedido.OrcamentoSolicitado);
                break;

            case MessageCategory.Pedido:
                CriarPedidoSeNaoExistir(cliente.Id, StatusPedido.Prospecao);
                break;

            case MessageCategory.Suporte:
                _logger.LogInformation(
                    "Mensagem classificada como SUPORTE | Cliente={ClienteId}",
                    cliente.Id);
                break;

            case MessageCategory.Duvida:
                _logger.LogInformation(
                    "Mensagem classificada como DUVIDA | Cliente={ClienteId}",
                    cliente.Id);
                break;

            default:
                _logger.LogInformation(
                    "Mensagem classificada como OUTROS | Cliente={ClienteId}",
                    cliente.Id);
                break;
        }

        await _context.SaveChangesAsync();

        return Ok(new
        {
            categoria = category.ToString()
        });
    }

    private void CriarPedidoSeNaoExistir(Guid clienteId, StatusPedido status)
    {
        var existePedidoAberto = _context.Pedidos.Any(p =>
            p.ClienteId == clienteId &&
            p.Status == status);

        if (existePedidoAberto)
            return;

        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            ClienteId = clienteId,
            TipoEvento = "A definir",
            ValorTotal = 0,
            Status = status,
            StatusPagamento = StatusPagamento.Nenhum,
            DataCriacao = DateTime.UtcNow
        };

        _context.Pedidos.Add(pedido);
    }
}
