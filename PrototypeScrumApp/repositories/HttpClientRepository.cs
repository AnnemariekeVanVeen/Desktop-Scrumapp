using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PrototypeScrumApp.repositories
{
    public class HttpClientRepository
    {
        public HttpClient Client { get; set; }

        public HttpClientRepository()
        {
            Client = new HttpClient { BaseAddress = new Uri(Properties.Settings.Default.Api_url) };
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(Properties.Settings.Default.Application_json));

            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.active_api_token);
        }
    }
}
