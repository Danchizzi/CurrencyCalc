using CurrencyCalc.App.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyCalc.Infrastructure
{
    public static class DependencyRegistrations
    {
        public static IServiceCollection AddInfranstucture(this IServiceCollection services, string fileName)
        {
            services.AddSingleton<IRatesDataProvider>(new XmlRatesDataProvider(fileName));
            return services;
        }
    }
}
