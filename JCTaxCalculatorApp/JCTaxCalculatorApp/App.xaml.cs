using JCTaxCalculatorApp.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xamarin.Forms;
using JC.Common.Interfaces;
using JC.Common.Models;
using JC.Common.Services;
using JCTaxCalculatorApp.ViewModels;
using System.Collections.Generic;

namespace JCTaxCalculatorApp
{
    public partial class App : Application
    {
        public Order Order { get; private set; }

        public new static App Current => (App)Application.Current;

        public IServiceProvider ServiceProvider { get; }

        public App()
        {
            var services = new ServiceCollection();
            // Services
            services.AddSingleton<ITaxCalculator, TaxJarTaxCalculator>();
            services.AddSingleton<ITaxService, TaxService>();
            // ViewModels
            services.AddTransient<OrderViewModel>();

            ServiceProvider = services.BuildServiceProvider();

            InitializeComponent();

            // create a dummy Customer/Items/Order for display on the OrderPage
            var candy1 = new Item
            {
                ItemName = "Everlasting Gobstoppers",
                Price = 3.50M,
                Quantity = 10,
            };
            var candy2 = new Item
            {
                ItemName = "Lickable Wallpaper",
                Price = 19.99M,
                Quantity = 1,
            };
            var candy3 = new Item
            {
                ItemName = "Fizzy Lifting Drinks",
                Price = 2.50M,
                Quantity = 4,
            };

            var address = new Location
            {
                ZipCode = "13413",
                City = "New Hartford",
                State = "NY",
                Country = "US",
                StreetAddress = "1 Tennyson Circle"
            };

            var customer = new Customer
            {
                Name = "TomK",
                BillingAddress = address,
                HomeAddress = address,
                MailingAddress = address,
            };

            Order = new Order 
            { 
                Customer = customer, 
                Items = new List<Item>(new Item[] { candy1, candy2, candy3 }), 
            };

            MainPage = new OrderPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
