using JC.Common.Interfaces;
using JC.Common.Models;
using System;

namespace JCTaxCalculatorApp.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        private string _taxRate;
        public string TaxRate
        {
            get => _taxRate;
            set
            {
                SetProperty(ref _taxRate, value);
                // todo: 
                // OnPropertyChanged(nameof(TotalTax));
            }
        }

        private string _zipCode;
        public string ZipCode
        {
            get => _zipCode;
            set => SetProperty(ref _zipCode, value);
        }

        ITaxService _taxService;

        private event EventHandler InitializeVM = delegate { };

        public OrderViewModel(ITaxService taxService)
        {
            _taxService = taxService;

            InitializeVM += Initialize;
            // raise event
            InitializeVM(this, EventArgs.Empty);
        }

        private async void Initialize(object sender, EventArgs e)
        {
            InitializeVM -= Initialize;
            decimal taxRate = await _taxService.GetTaxRateAsync(new Location { ZipCode = "12345" });
            TaxRate = taxRate.ToString();
        }
    }
}
