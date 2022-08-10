using CurrencyCalc.App.Interfaces;
using CurrencyCalc.App.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyCalc.App
{
    public static class DependencyRegistrations
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IConvertService, ConvertService>();
            return services;
        }
    }
}
