using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;

namespace JC.Common.Interfaces
{
    public interface IHttpClientService
    {
        HttpClient GetClient();

        NameValueCollection BuildQueryString(Dictionary<string, string> queryParams);
    }
}
