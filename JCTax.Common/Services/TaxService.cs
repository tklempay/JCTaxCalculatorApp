using JC.Common.Interfaces;
using JC.Common.Models;
using System;
using System.Threading.Tasks;

namespace JC.Common.Services
{
    public class TaxService : ITaxService
    {
        private readonly ITaxCalculator _taxCalculator;

        public TaxService(ITaxCalculator taxCalculator)
        {
            _taxCalculator = taxCalculator;
        }

        public async Task<decimal> GetTaxRateAsync(Location location)
        {
            var taxRate = await _taxCalculator.GetTaxRateAsync(location);
            return taxRate;
        }

        public async Task<decimal> CalculateTaxesAsync(Order order)
        {
            var totalTaxes = await _taxCalculator.CalculateTaxesAsync();
            return totalTaxes;
        }
    }
}
