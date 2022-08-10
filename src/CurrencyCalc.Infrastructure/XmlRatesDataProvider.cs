using CurrencyCalc.App.Interfaces;
using CurrencyCalc.App.Models;
using System.Globalization;
using System.Xml.Linq;

namespace CurrencyCalc.Infrastructure
{
    public class XmlRatesDataProvider : IRatesDataProvider
    {
        IReadOnlyCollection<ExchangeRate> _rates;

        public XmlRatesDataProvider(string fileName)
        {
            _rates = ParseRates(fileName);
        }

        public IReadOnlyCollection<ExchangeRate> GetRates() => _rates;

        private IReadOnlyCollection<ExchangeRate> ParseRates(string fileName)
        {
            string filepath = GetFilepath(fileName);
            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException();
            }
            XDocument xml = XDocument.Load(filepath);
            XNamespace gesmes = "http://www.gesmes.org/xml/2002-08-01";
            XName cubeName = XName.Get("Cube", "http://www.ecb.int/vocabulary/2002-08-01/eurofxref");
            var rates = xml.Root?.Element(cubeName)?.Element(cubeName)?.Elements(cubeName)
                .Select(ParseToExchangeRate)
                .ToArray();
            if (rates == null)
            {
                throw new Exception("Wrong file format");
            }
            return rates;
        }

        private ExchangeRate ParseToExchangeRate(XElement element)
        {
            string? currency = element.Attribute("currency")?.Value;
            if(currency == null)
            {
                throw new Exception("Currency is not set");
            }
            string? rateString = element.Attribute("rate")?.Value;
            if(rateString == null)
            {
                throw new Exception("Rate is not set");
            }
            if (!decimal.TryParse(rateString, NumberStyles.Float | NumberStyles.AllowThousands, new NumberFormatInfo { NumberDecimalSeparator = "."}, out var rate))
            {
                throw new Exception($"Can't convert {rateString} to number");
            }
            return new ExchangeRate(currency, rate);
        }

        private string GetFilepath(string filename)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var filepath = Path.Combine(currentDirectory, filename);
            return filepath;
        }
    }
}