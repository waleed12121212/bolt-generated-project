using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazingo_Core.DomainLogic
{
    public class TaxCalculator
    {
        public decimal CalculateTax(decimal amount , decimal taxRate)
        {
            return amount * taxRate;
        }
    }
}
