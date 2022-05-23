using JC.Common.Interfaces;
using JC.Common.Services;
using JC.Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace JC.Common.Tests
{
    [TestClass]
    public class TaxJarUnitTest
    {
        private Item Item { get; }

        private Customer Customer { get; set; }

        private IServiceProvider ServiceProvider { get; }
        public TaxJarUnitTest()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ITaxCalculator, TaxJarTaxCalculator>();
            services.AddSingleton<ITaxService, TaxService>();
            services.AddSingleton<IHttpClientService, HttpClientService>();
            ServiceProvider = services.BuildServiceProvider();

            Item = new()
            {
                ItemName = "Widget",
                Description = "Best Widgets In The World!",
                Price = 10.00M,
                Quantity = 1,
            };

            Location address = new()
            {
                ZipCode = "13413",
                City = "New Hartford",
                State = "NY",
                Country = "US",
                StreetAddress = "1 Tennyson Circle"
            };

            Customer = new()
            {
                Name = "TomK",
                BillingAddress = address,
                HomeAddress = address,
                MailingAddress = address,
            };
        }

        [TestMethod]
        public async Task GetTaxRateNewHartfordNY()
        {
            Location location = new() { ZipCode = "13413", City = "New Hartford", Country = "US" };
            ITaxService taxService = ServiceProvider.GetService<ITaxService>();
            decimal taxRate = -1;
            try
            {
                taxRate = await taxService.GetTaxRateAsync(location);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            Assert.IsNotNull(taxRate);
            Assert.AreEqual(0.0875M, taxRate);
            Console.WriteLine($"TaxRate for {location.City}: {taxRate}");
        }

        [TestMethod]
        public async Task GetTaxRateLocationNull()
        {
            Location location = null;
            ITaxService taxService = ServiceProvider.GetService<ITaxService>();
            decimal taxRate = -1;
            try
            {
                taxRate = await taxService.GetTaxRateAsync(location);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Assert.AreEqual(-1, taxRate);
        }

        [TestMethod]
        public async Task GetTaxRateBadZipCode()
        {
            List<string> zipCodes = new List<string> { "", null, "0123", "1" };
            // todo: finish this test
        }

        [TestMethod]
        public async Task CalculateTaxSingleItem()
        {
            Order order = new()
            {
                Customer = Customer,
                Items = new List<Item> { Item },
            };
            ITaxService taxService = ServiceProvider.GetService<ITaxService>();
            decimal taxTotal = 0;
            try
            {
                taxTotal = await taxService.CalculateTaxesAsync(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Assert.IsNotNull(taxTotal);
            var singleItemNewHartfordTaxRate = Math.Round(Item.Price * .0875M, 2);
            Assert.AreEqual(singleItemNewHartfordTaxRate, taxTotal);
            Console.WriteLine($"TaxTotal: {taxTotal}");
        }
    }
}
