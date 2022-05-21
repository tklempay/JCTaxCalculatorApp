using JC.Common.Models;
using System.Threading.Tasks;

namespace JC.Common.Interfaces
{
    public interface ITaxService
    {
        /// <summary>
        /// GetTaxRate - returns the tax rate per the zipcode/city of the Location parameter
        /// </summary>
        /// <param name="location"></param>
        /// <returns>tax rate in decimal format, -1 if an error occurs</returns>
        Task<decimal> GetTaxRateAsync(Location location);

        /// <summary>
        /// CalculateTaxes - calculates the taxes for the Order
        /// </summary>
        /// <param name="order"></param>
        /// <returns>total tax amount, 0 if an error occurs</returns>
        Task<decimal> CalculateTaxesAsync(Order order);
    }
}
