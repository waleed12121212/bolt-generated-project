    using Bazingo_Application.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Bazingo_Core.Entities;
    using Bazingo_Core.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Bazingo_Core;

    namespace Bazingo_API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        [Authorize]
        public class CurrencyController : ControllerBase
        {
            private readonly ICurrencyRepository _currencyRepository;
            private readonly CurrencyService _currencyService;
            private readonly ILogger<CurrencyController> _logger;

            public CurrencyController(ICurrencyRepository currencyRepository, CurrencyService currencyService, ILogger<CurrencyController> logger)
            {
                _currencyRepository = currencyRepository;
                _currencyService = currencyService;
                _logger = logger;
            }

            [HttpGet]
            public async Task<IActionResult> GetAllCurrencies()
            {
                try
                {
                    var currencies = await _currencyRepository.GetAllAsync();
                    return Ok(currencies);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting all currencies");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetCurrencyById(int id)
            {
                try
                {
                    var currency = await _currencyRepository.GetByIdAsync(id);
                    if (currency == null) return NotFound();
                    return Ok(currency);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting currency by ID: {CurrencyId}", id);
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPost]
            [Authorize(Roles = Constants.Roles.Admin)]
            public async Task<IActionResult> CreateCurrency([FromBody] Currency currency)
            {
                if (currency == null)
                {
                    return BadRequest("Currency object is required.");
                }

                try
                {
                    await _currencyRepository.AddAsync(currency);
                    return CreatedAtAction(nameof(GetCurrencyById), new { id = currency.Id }, currency);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating currency");
                    return StatusCode(500, "Internal Server Error");
                }
            }
        }
    }
