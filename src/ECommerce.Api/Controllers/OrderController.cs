using System.Security.Claims;
using ECommerce.Application.UseCases.Orders.Commands;
using ECommerce.Application.UseCases.Orders.Dtos;
using ECommerce.Application.UseCases.Orders.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly ICreateOrderUseCase _createOrderUseCase;
    private readonly IGetOrderByIdUseCase _getOrderByIdUseCase;
    private readonly IGetOrdersByUserUseCase _getOrdersByUserUseCase;

    public OrderController(
        ICreateOrderUseCase createOrderUseCase,
        IGetOrderByIdUseCase getOrderByIdUseCase,
        IGetOrdersByUserUseCase getOrdersByUserUseCase)
    {
        _createOrderUseCase = createOrderUseCase;
        _getOrderByIdUseCase = getOrderByIdUseCase;
        _getOrdersByUserUseCase = getOrdersByUserUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetMine(CancellationToken ct)
    {
        var userId = GetUserId();
        var orders = await _getOrdersByUserUseCase.ExecuteAsync(userId, ct);

        return Ok(orders);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var order = await _getOrderByIdUseCase.ExecuteAsync(id, ct);

        if (order is null)
        {
            return NotFound();
        }

        if (!User.IsInRole("Admin") && order.UserId != GetUserId())
        {
            return Forbid();
        }

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateOrderRequest request,
        CancellationToken ct)
    {
        try
        {
            var order = await _createOrderUseCase.ExecuteAsync(
                new CreateOrderCommand(GetUserId(), request),
                ct);

            return CreatedAtAction(
                nameof(GetById),
                new { id = order.Id },
                order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    private Guid GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(value, out var userId)
            ? userId
            : throw new InvalidOperationException("Invalid user token");
    }
}
