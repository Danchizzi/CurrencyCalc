using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyCalc.App.Models
{
    public class ExchangeRate
    {
        public ExchangeRate(string currency, decimal rate)
        {
            Currency = currency;
            Rate = rate;
        }

        public string Currency { get; }
        public decimal Rate { get; }
    }
}
