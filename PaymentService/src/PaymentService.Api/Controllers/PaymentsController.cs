using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Commands;
using PaymentService.Application.DTOs;

namespace PaymentService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("process")]
    [ProducesResponseType(typeof(PaymentResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaymentResponseDto>> Process(
        PaymentRequestDto request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new ProcessPaymentCommand(request),
            cancellationToken);

        return Ok(response);
    }
}
