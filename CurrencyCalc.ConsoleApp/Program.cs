// See https://aka.ms/new-console-template for more information
using CurrencyCalc.App;
using CurrencyCalc.App.Interfaces;
using CurrencyCalc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection()
    .AddServices()
    .AddInfranstucture("eurofxref-daily.xml");
var serviceProvider = services.BuildServiceProvider();
var convertService = serviceProvider.GetRequiredService<IConvertService>();
Console.WriteLine(convertService.ConvertEurosTo("USD", 5));