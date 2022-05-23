using JC.Common.Interfaces;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;

namespace JC.Common.Services
{
    public class HttpClientService : IHttpClientService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public HttpClient GetClient() => _httpClient;

        public NameValueCollection BuildQueryString(Dictionary<string, string> queryParams)
        {
            // build up the query string params (if there are any)
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            foreach (var item in queryParams)
            {
                if (string.IsNullOrWhiteSpace(item.Value) == false)
                {
                    queryString[item.Key] = item.Value;
                }
            }

            return queryString;
        }
    }
}
