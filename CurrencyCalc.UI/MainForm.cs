using CurrencyCalc.App;
using CurrencyCalc.App.Interfaces;
using CurrencyCalc.App.Models;
using CurrencyCalc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyCalc.UI
{
    public partial class MainForm : Form
    {
        private readonly IConvertService convertService;
        public MainForm()
        {
            InitializeComponent();
            try
            {
                var services = new ServiceCollection()
                .AddServices()
                .AddInfranstucture("eurofxref-daily.xml");
                var serviceProvider = services.BuildServiceProvider();
                convertService = serviceProvider.GetRequiredService<IConvertService>();
                var rates = serviceProvider.GetRequiredService<IRatesDataProvider>().GetRates();
                InitializeCurrencyComboBox(rates);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error : {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void InitializeCurrencyComboBox(IReadOnlyCollection<ExchangeRate> rates) 
        {
            var currencies = rates.Select(r => r.Currency).ToArray();
            currencyComboBox.Items.AddRange(currencies);
        }

        private void currencyComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            Reculculate();
        }

        private void amountNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Reculculate();
        }

        private void Reculculate()
        {
            try
            {
                var currency = currencyComboBox.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(currency))
                {
                    resultLabel.Text = "";
                    return;
                }
                var amount = Math.Round(amountNumericUpDown.Value, 2);
                var result = convertService.ConvertEurosTo(currency, amount);
                resultLabel.Text = $"{amount} EUROS = {result} {currency}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error : {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}