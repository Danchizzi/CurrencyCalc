using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyCalc.App.Interfaces
{
    public interface IConvertService
    {
        decimal ConvertEurosTo(string currency, decimal amount);
    }
}
