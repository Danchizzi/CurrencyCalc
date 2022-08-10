using CurrencyCalc.App.Models;
using CurrencyCalc.Infrastructure;
using FluentAssertions;

namespace CurrencyCalc.Tests
{
    public class XmlRatesDataProviderTests
    {
        private const string TestFileName = "testRates.xml";
        private XmlRatesDataProvider sut = null!;

        public XmlRatesDataProviderTests()
        {
            File.Delete(TestFileName);
        }

        [Fact]
        public void GetRates_ValidXml_RatesAreReturned()
        {
			//Arrange
			SaveTestXml(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<gesmes:Envelope xmlns:gesmes=""http://www.gesmes.org/xml/2002-08-01"" xmlns=""http://www.ecb.int/vocabulary/2002-08-01/eurofxref"">
	<gesmes:subject>Reference rates</gesmes:subject>
	<gesmes:Sender>
		<gesmes:name>European Central Bank</gesmes:name>
	</gesmes:Sender>
	<Cube>
		<Cube time=""2022-08-08"">
			<Cube currency=""USD"" rate=""1.0199""/>
			<Cube currency=""PLN"" rate=""4.7043""/>
		</Cube>
	</Cube>
</gesmes:Envelope>");

            sut = new XmlRatesDataProvider(TestFileName);

            //Act
            var result = sut.GetRates();

            //Assert
            var expected = new[]
            {
                new ExchangeRate("USD", 1.0199m),
                new ExchangeRate("PLN", 4.7043m)
            };
            result.Should().BeEquivalentTo(expected);
        }

        private void SaveTestXml(string content)
        {
            File.WriteAllText(TestFileName, content);
        }


        [Fact]
        public void XmlRatesDataProviderCtor_WrongFileFormat_ThrownException()
        {
            //Arrange
            SaveTestXml(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<gesmes:Envelope xmlns:gesmes=""http://www.gesmes.org/xml/2002-08-01"" xmlns=""http://www.ecb.int/vocabulary/2002-08-01/eurofxref"">
	<gesmes:subject>Reference rates</gesmes:subject>
	<gesmes:Sender>
		<gesmes:name>European Central Bank</gesmes:name>
	</gesmes:Sender>
	<Cube>
		
	</Cube>
</gesmes:Envelope>");

            //Act
            Action action = () => new XmlRatesDataProvider(TestFileName);

            //Assert
            action.Should().Throw<Exception>().WithMessage("Wrong file format");
        }

        [Fact]
        public void XmlRatesDataProviderCtor_CurrecyIsNotSet_ThrownException()
        {
            //Arrange
            SaveTestXml(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<gesmes:Envelope xmlns:gesmes=""http://www.gesmes.org/xml/2002-08-01"" xmlns=""http://www.ecb.int/vocabulary/2002-08-01/eurofxref"">
	<gesmes:subject>Reference rates</gesmes:subject>
	<gesmes:Sender>
		<gesmes:name>European Central Bank</gesmes:name>
	</gesmes:Sender>
	<Cube>
		<Cube time=""2022-08-08"">
			<Cube rate=""1.0199""/>
		</Cube>
	</Cube>
</gesmes:Envelope>");

            //Act
            Action action = () => new XmlRatesDataProvider(TestFileName);

            //Assert
            action.Should().Throw<Exception>().WithMessage("Currency is not set");
        }

        [Fact]
        public void XmlRatesDataProviderCtor_RateIsNotSet_ThrownException()
        {
            //Arrange
            SaveTestXml(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<gesmes:Envelope xmlns:gesmes=""http://www.gesmes.org/xml/2002-08-01"" xmlns=""http://www.ecb.int/vocabulary/2002-08-01/eurofxref"">
	<gesmes:subject>Reference rates</gesmes:subject>
	<gesmes:Sender>
		<gesmes:name>European Central Bank</gesmes:name>
	</gesmes:Sender>
	<Cube>
		<Cube time=""2022-08-08"">
			<Cube currency=""USD""/>
		</Cube>
	</Cube>
</gesmes:Envelope>");

            //Act
            Action action = () => new XmlRatesDataProvider(TestFileName);

            //Assert
            action.Should().Throw<Exception>().WithMessage("Rate is not set");
        }

        [Fact]
        public void XmlRatesDataProviderCtor_RateIsNotANumber_ThrownException()
        {
            //Arrange
            SaveTestXml(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<gesmes:Envelope xmlns:gesmes=""http://www.gesmes.org/xml/2002-08-01"" xmlns=""http://www.ecb.int/vocabulary/2002-08-01/eurofxref"">
	<gesmes:subject>Reference rates</gesmes:subject>
	<gesmes:Sender>
		<gesmes:name>European Central Bank</gesmes:name>
	</gesmes:Sender>
	<Cube>
		<Cube time=""2022-08-08"">
			<Cube currency=""USD"" rate=""www""/>
		</Cube>
	</Cube>
</gesmes:Envelope>");

            //Act
            Action action = () => new XmlRatesDataProvider(TestFileName);

            //Assert
            action.Should().Throw<Exception>().WithMessage("Can't convert www to number");
        }

    }

    
}