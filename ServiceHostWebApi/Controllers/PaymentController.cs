using MasterManagment.Application.Contracts.Payment;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentApplication _paymentApplication;

    public PaymentController(IPaymentApplication paymentApplication)
    {
        _paymentApplication = paymentApplication;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreatePaymentCommand command)
    {
        // ورودی شامل شناسه Cart است، نه OrderId
        try
        {
            var paymentId = await _paymentApplication.CreateAsync(command);
            return Ok(new { PaymentId = paymentId });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.InnerException?.Message ?? ex.Message);
        }
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Edit([FromBody] EditPaymentCommand command)
    {
        try
        {
            await _paymentApplication.UpdateAsync(command);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("cancel/{id}")]
    public async Task<IActionResult> Cancel(long id)
    {
        try
        {
            await _paymentApplication.CancelAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            var payment = await _paymentApplication.GetByIdAsync(id);
            if (payment == null) return NotFound();

            return Ok(payment);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // تغییر مسیر برای هماهنگی با منطق شما: دریافت پرداخت‌ها بر اساس CartId
    [HttpGet("cart/{cartId}")]
    public async Task<IActionResult> GetByCartId(long cartId)
    {
        try
        {
            var payments = await _paymentApplication.GetByCartIdAsync(cartId);
            return Ok(payments);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] PaymentSearchCriteria criteria)
    {
        try
        {
            var payments = await _paymentApplication.SearchAsync(criteria);
            return Ok(payments);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
