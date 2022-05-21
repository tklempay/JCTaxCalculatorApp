using JC.Common.Interfaces;
using JC.Common.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Collections.Specialized;
using System.Net.Http.Json;

namespace JC.Common.Services
{
    public class TaxJarTaxCalculator : ITaxCalculator
    {
        // API key to access Tax Jar sales tax API
        private string _apiKey => "5da2f821eee4035db4771edab942a4cc";
        
        // example of complete URL: https://api.taxjar.com/v2/rates/13413?country=US&city=New Hartford

        private string _baseAPIURL = "https://api.taxjar.com/v2/rates/";

        private static readonly HttpClient _httpClient = new HttpClient();

        public TaxJarTaxCalculator()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
        }

        /// <summary>
        /// GetTaxRate - queries the TaxJar api for the tax rate per the zipcode/city of the Location parameter
        /// </summary>
        /// <param name="location"></param>
        /// <returns>tax rate in decimal format, -1 if an error occurs</returns>
        public async Task<decimal> GetTaxRateAsync(Location location)
        {
            // todo: add a separate HttpService
            decimal taxRate = -1;
            try
            {
                if (location == null)
                {
                    throw new ArgumentNullException(nameof(location));
                }
                // todo: better solution is to build a service that does ZipCode validation but for this test, keep it simple
                else if (location.ZipCode?.Length < 5 || int.TryParse(location.ZipCode, out _) == false)
                {
                    throw new ArgumentException($"Bad argument (ZipCode): {location.ZipCode}", "Location.ZipCode");
                }

                // build up the query string params (if there are any)
                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
                if (string.IsNullOrWhiteSpace(location.Country) == false)
                {
                    queryString.Add("country", location.Country);
                }
                if (string.IsNullOrWhiteSpace(location.City) == false)
                {
                    queryString.Add("city", location.City);
                }
                
                // build up the URL to call the tax API
                var apiURL = new UriBuilder($"{_baseAPIURL}{location.ZipCode}")
                {
                    Query = queryString.ToString(),
                };
                var results = await _httpClient.GetFromJsonAsync<TaxRate>(apiURL.Uri);
                taxRate = decimal.Parse(results.Rate.CombinedRate);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return taxRate;
        }

        public Task<decimal> CalculateTaxesAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
