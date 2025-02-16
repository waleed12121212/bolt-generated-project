using Bazingo_Application.DTOs.Payments;
using Bazingo_Core.DomainLogic;
using Bazingo_Core.Models;
using Bazingo_Core.Entities.Payment;
using Bazingo_Core.Enums;
using Bazingo_Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bazingo_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly PaymentProcessor _paymentProcessor;

        public PaymentController(IPaymentRepository paymentRepository, PaymentProcessor paymentProcessor)
        {
            _paymentRepository = paymentRepository;
            _paymentProcessor = paymentProcessor;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] Payment payment)
        {
            var result = await _paymentProcessor.ProcessPayment(payment);
            if (result)
            {
                return Ok(new { message = "Payment processed successfully" });
            }
            return BadRequest(new { message = "Payment processing failed" });
        }
    }
}
