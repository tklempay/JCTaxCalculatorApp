using JC.Common.Interfaces;
using JC.Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

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
        private event EventHandler CalculateTotalTax = delegate { };

        private decimal _totalTax;
        public decimal TotalTax
        {
            get => _totalTax;
            set => SetProperty(ref _totalTax, value);
        }

        private string _zipCode;
        public string ZipCode
        {
            get => _zipCode;
            set => SetProperty(ref _zipCode, value);
        }

        private Order _order;
        public Order Order 
        { 
            get => _order;
            set
            {
                SetProperty(ref _order, value);
                ZipCode = _order.Customer.MailingAddress.ZipCode;
            }
        }

        private ITaxService _taxService;

        public OrderViewModel(ITaxService taxService)
        {
            _taxService = taxService;
            Order = App.Current.Order;
            CalculateTotalTax += GetTotalTaxAsync;
            // raise the event
            CalculateTotalTax(this, EventArgs.Empty);
        }

        private async void GetTotalTaxAsync(object sender, EventArgs e)
        {
            CalculateTotalTax -= GetTotalTaxAsync;
            try
            {
                TotalTax = await _taxService.CalculateTaxesAsync(Order).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                TotalTax = 0;
                await Application.Current.MainPage.DisplayAlert("Order Page",
                                                                $"Error occurred while calculating the total tax on your order: {ex.Message}",
                                                                "OK");
            }
        }
    }
}
