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
        private event EventHandler CalculateOrder = delegate { };

        private decimal _totalTax;
        public decimal TotalTax
        {
            get => _totalTax;
            set => SetProperty(ref _totalTax, value);
        }

        private decimal _orderTotal;
        public decimal OrderTotal
        {
            get => _orderTotal;
            set => SetProperty(ref _orderTotal, value);
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
            CalculateOrder += GetTotalsAsync;
            // raise the event
            CalculateOrder(this, EventArgs.Empty);
        }

        private async void GetTotalsAsync(object sender, EventArgs e)
        {
            CalculateOrder -= GetTotalsAsync;
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
            finally
            {
                OrderTotal = Order.OrderTotal + TotalTax;
            }
        }
    }
}
