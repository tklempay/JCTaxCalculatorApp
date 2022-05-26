using JC.Common.Interfaces;
using JC.Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace JCTaxCalculatorApp.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        private decimal _taxRate;
        public decimal TaxRate
        {
            get => _taxRate;
            set => SetProperty(ref _taxRate, value);
        }

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

        private IEnumerable<Item> _orderItems;
        public IEnumerable<Item> OrderItems
        {
            get => _orderItems;
            set => SetProperty(ref _orderItems, value);
        }

        private ITaxService _taxService;

        private event EventHandler InitializeVM = delegate { };

        public OrderViewModel(ITaxService taxService)
        {
            _taxService = taxService;
            Order = App.Current.Order;
            InitializeVM += Initialize;
            // raise the event
            InitializeVM(this, EventArgs.Empty);
        }

        // poor man's life cycle event for view model loading - done for async
        private async void Initialize(object sender, EventArgs e)
        {
            InitializeVM -= Initialize;

            await SetTaxes().ConfigureAwait(false);
            OrderItems = Order.Items;
            OrderTotal = Order.OrderTotal + TotalTax;
        }

        public IAsyncCommand PlaceOrderCommand => new AsyncCommand(async () =>
        {
            await Application.Current.MainPage.DisplayAlert("JCTest - TomK",
                                                            "Thank you for placing your order!",
                                                            "OK");
        });

        public IAsyncCommand UpdateZipcodeCommand => new AsyncCommand(async () =>
        {
            Order.Customer.MailingAddress.ZipCode = ZipCode;
            // real world example would prompt for full address, not just zipcode.  for now, just clear it out
            Order.Customer.MailingAddress.City = String.Empty;
            Order.Customer.MailingAddress.StreetAddress = String.Empty;
            await SetTaxes().ConfigureAwait(false);
            OrderTotal = Order.OrderTotal + TotalTax;
        });

        /// <summary>
        /// SetTaxes - private method to query the tax service for the tax rate and update the tax totals
        /// </summary>
        /// <returns></returns>
        private async Task SetTaxes()
        {
            try
            {
                TaxRate = await _taxService.GetTaxRateAsync(Order.Customer.MailingAddress);
                TotalTax = await _taxService.CalculateTaxesAsync(Order).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                TotalTax = 0;
                await Application.Current.MainPage.DisplayAlert("Order Page",
                                                                $"Error occurred while calculating the total tax on your order: {ex.Message}",
                                                                "OK");
            }
        }
    }
}
