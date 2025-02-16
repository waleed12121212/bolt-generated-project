    using Bazingo_Application.DTOs.Payments;
    using Bazingo_Core.DomainLogic;
    using Bazingo_Core.Models;
    using Bazingo_Core.Entities.Payment;
    using Bazingo_Core.Enums;
    using Bazingo_Core.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Bazingo_Core;

    namespace Bazingo_API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        [Authorize]
        public class PaymentController : ControllerBase
        {
            private readonly IPaymentRepository _paymentRepository;
            private readonly PaymentProcessor _paymentProcessor;
            private readonly ILogger<PaymentController> _logger;

            public PaymentController(IPaymentRepository paymentRepository, PaymentProcessor paymentProcessor, ILogger<PaymentController> logger)
            {
                _paymentRepository = paymentRepository;
                _paymentProcessor = paymentProcessor;
                _logger = logger;
            }

            [HttpPost]
            public async Task<IActionResult> ProcessPayment([FromBody] Payment payment)
            {
                if (payment == null)
                {
                    return BadRequest("Payment object is required.");
                }

                try
                {
                    var result = await _paymentProcessor.ProcessPayment(payment);
                    if (result)
                    {
                        return Ok(new { message = "Payment processed successfully" });
                    }
                    return BadRequest(new { message = "Payment processing failed" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing payment");
                    return StatusCode(500, "Internal Server Error");
                }
            }
        }
    }
