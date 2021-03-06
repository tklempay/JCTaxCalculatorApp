using System.Threading.Tasks;
using JC.Common.Models;

namespace JC.Common.Interfaces
{
    public interface ITaxCalculator
    {
        /// <summary>
        /// GetTaxRate - returns the tax rate per the zipcode/city of the Location parameter
        /// </summary>
        /// <param name="location"></param>
        /// <returns>tax rate in decimal format, -1 if an error occurs</returns>
        Task<decimal> GetTaxRateAsync(Location location);
        Task<decimal> CalculateTaxesAsync();
    }
}
