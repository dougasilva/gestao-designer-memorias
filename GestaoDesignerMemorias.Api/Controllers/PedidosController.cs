using GestaoDesignerMemorias.Domain.Enums;
using GestaoDesignerMemorias.Domain.Entities;
using GestaoDesignerMemorias.DTOs.Pedidos;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerMemorias.Controllers;

[ApiController]
[Route("api/pedidos")]
public class PedidosController : ControllerBase
{
    private readonly AppDbContext _context;

    public PedidosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/pedidos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PedidoResponseDto>>> GetAll()
    {
        var pedidos = await _context.Pedidos
            .Select(p => new PedidoResponseDto
            {
                Id = p.Id,
                ClienteId = p.ClienteId,
                TipoEvento = p.TipoEvento,
                ValorTotal = p.ValorTotal,
                Status = p.Status,
                StatusPagamento = p.StatusPagamento,
                DataCriacao = p.DataCriacao
            })
            .ToListAsync();

        return Ok(pedidos);
    }

    // GET: api/pedidos/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PedidoResponseDto>> GetById(Guid id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);

        if (pedido == null)
            return NotFound();

        var response = new PedidoResponseDto
        {
            Id = pedido.Id,
            ClienteId = pedido.ClienteId,
            TipoEvento = pedido.TipoEvento,
            ValorTotal = pedido.ValorTotal,
            Status = pedido.Status,
            StatusPagamento = pedido.StatusPagamento,
            DataCriacao = pedido.DataCriacao
        };

        return Ok(response);
    }

    // POST: api/pedidos
    [HttpPost]
    public async Task<ActionResult<PedidoResponseDto>> Create(PedidoCreateDto dto)
    {
        var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == dto.ClienteId);
        if (!clienteExiste)
            return BadRequest("Cliente não encontrado.");

        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            ClienteId = dto.ClienteId,
            TipoEvento = dto.TipoEvento,
            ValorTotal = dto.ValorTotal,
            Status = StatusPedido.Prospecao,
            StatusPagamento = StatusPagamento.Nenhum,
            DataCriacao = DateTime.UtcNow
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        var response = new PedidoResponseDto
        {
            Id = pedido.Id,
            ClienteId = pedido.ClienteId,
            TipoEvento = pedido.TipoEvento,
            ValorTotal = pedido.ValorTotal,
            Status = pedido.Status,
            StatusPagamento = StatusPagamento.Pago,
            DataCriacao = pedido.DataCriacao
        };

        return CreatedAtAction(nameof(GetById), new { id = pedido.Id }, response);
    }

    // PUT: api/pedidos/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, PedidoUpdateDto dto)
    {
        var pedido = await _context.Pedidos.FindAsync(id);

        if (pedido == null)
            return NotFound();

        pedido.Status = dto.Status;
        pedido.StatusPagamento = dto.StatusPagamento;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/pedidos/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);

        if (pedido == null)
            return NotFound();

        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
