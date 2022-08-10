using CurrencyCalc.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyCalc.App.Services
{
    public class ConvertService : IConvertService
    {
        private readonly IRatesDataProvider _ratesDataProvider;

        public ConvertService(IRatesDataProvider ratesDataProvider)
        {
            _ratesDataProvider = ratesDataProvider;
        }
        public decimal ConvertEurosTo(string currency, decimal amount)
        {
            var rate = _ratesDataProvider.GetRates().FirstOrDefault(rate => rate.Currency.Equals(currency));
            if(rate == null)
            { 
                throw new Exception("Rate was not found");
            }
            return Math.Round(rate.Rate * amount, 2);
        }
    }
}
