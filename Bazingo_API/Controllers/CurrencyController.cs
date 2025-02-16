using Bazingo_Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bazingo_Core.Entities;
using Bazingo_Core.Interfaces;

namespace Bazingo_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly CurrencyService _currencyService;

        public CurrencyController(ICurrencyRepository currencyRepository , CurrencyService currencyService)
        {
            _currencyRepository = currencyRepository;
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCurrencies( )
        {
            var currencies = await _currencyRepository.GetAllAsync();
            return Ok(currencies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCurrencyById(int id)
        {
            var currency = await _currencyRepository.GetByIdAsync(id);
            if (currency == null) return NotFound();
            return Ok(currency);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCurrency([FromBody] Currency currency)
        {
            await _currencyRepository.AddAsync(currency);
            return CreatedAtAction(nameof(GetCurrencyById) , new { id = currency.Id } , currency);
        }
    }
}
