using CurrencyCalc.App.Interfaces;
using CurrencyCalc.App.Models;
using CurrencyCalc.App.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyCalc.Tests
{

    public class ConvertServiceTests
    {
        private readonly ConvertService sut;

        public ConvertServiceTests()
        {
            var ratesDataProviderMock = new Mock<IRatesDataProvider>(MockBehavior.Strict);
            ratesDataProviderMock
                .Setup(m => m.GetRates())
                .Returns(() => new[]
                {
                    new ExchangeRate("USD", 1.0199m),
                    new ExchangeRate("PLN", 4.7043m)
                });
            sut = new ConvertService(ratesDataProviderMock.Object);
        }


        [Theory]
        [InlineData("USD", 5.1)]
        [InlineData("PLN", 23.52)]
        public void ConvertEurosTo_ValidParams_ExpectedResultReturned(string currency, decimal expected)
        {
            //Arrange

            //Act
            var result = sut.ConvertEurosTo(currency, 5);
            
            //Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void ConvertEurosTo_MissingCurrency_ThrownException()
        {
            //Arrange

            //Act
            Action action = () => sut.ConvertEurosTo("AUD", 5);

            //Assert
            action.Should().Throw<Exception>().WithMessage("Rate was not found");
        }
    }
}
