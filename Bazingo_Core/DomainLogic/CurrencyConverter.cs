using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazingo_Core.DomainLogic
{
    public class CurrencyConverter
    {
        private readonly Dictionary<string , decimal> _exchangeRates;

        public CurrencyConverter(Dictionary<string , decimal> exchangeRates)
        {
            _exchangeRates = exchangeRates;
        }

        public decimal Convert(decimal amount , string fromCurrency , string toCurrency)
        {
            if (!_exchangeRates.ContainsKey(fromCurrency) || !_exchangeRates.ContainsKey(toCurrency))
            {
                throw new InvalidOperationException("Invalid currency codes.");
            }

            var rate = _exchangeRates[toCurrency] / _exchangeRates[fromCurrency];
            return amount * rate;
        }
    }
}
