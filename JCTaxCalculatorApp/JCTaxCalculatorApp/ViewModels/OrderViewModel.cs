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
                OrderItems = Order.Items;
                OrderTotal = Order.OrderTotal + TotalTax;
            }
        }

        public IAsyncCommand PlaceOrderCommand => new AsyncCommand(async () =>
        {
            await Application.Current.MainPage.DisplayAlert("JCTest - TomK",
                                                            "Thank you for placing your order!",
                                                            "OK");
        });
    }
}
