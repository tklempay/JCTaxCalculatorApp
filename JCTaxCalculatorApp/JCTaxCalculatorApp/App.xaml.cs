using JCTaxCalculatorApp.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xamarin.Forms;
using JC.Common.Interfaces;
using JC.Common.Services;
using JCTaxCalculatorApp.ViewModels;

namespace JCTaxCalculatorApp
{
    public partial class App : Application
    {
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
