using Microsoft.AspNetCore.Mvc;
using Pay.Application.DTOs;
using Pay.Application.Services;

namespace Pay.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController(IPaymentService service) : ControllerBase
    {
        private readonly IPaymentService _service = service;

        [HttpPost]
        public IActionResult CreatePayment([FromBody] CreatePaymentRequest request)
        {
            var result = _service.Create(request);
            return Ok(result);
        }
    }
}
