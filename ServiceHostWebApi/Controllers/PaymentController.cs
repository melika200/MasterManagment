using MasterManagment.Application.Contracts.Payment;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHostWebApi.Controllers;

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
    [ProducesResponseType(200, Type = typeof(long))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreatePaymentCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

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
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Edit([FromBody] EditPaymentCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _paymentApplication.UpdateAsync(command);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

  
    [HttpPut("cancel/{id:long}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Cancel(long id)
    {
        try
        {
            await _paymentApplication.CancelAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

  
    [HttpGet("{id:long}")]
    [ProducesResponseType(200, Type = typeof(PaymentViewModel))]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            var payment = await _paymentApplication.GetByIdAsync(id);
            if (payment == null)
                return NotFound();

            return Ok(payment);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    ///  // دریافت لیست پرداخت‌ها بر اساس شناسه کارت
    /// </summary>
    /// <param name="cartId"></param>
    /// <returns></returns>
    [HttpGet("cart/{cartId:long}")]
    [ProducesResponseType(200, Type = typeof(List<PaymentViewModel>))]
    [ProducesResponseType(400)]
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
    [ProducesResponseType(200, Type = typeof(List<PaymentViewModel>))]
    [ProducesResponseType(400)]
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
