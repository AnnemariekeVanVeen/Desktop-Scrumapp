using System.Net.Http;

namespace PrototypeScrumApp.repositories
{
    public class ApiRepository : HttpClientRepository
    {
        /// <summary>
        /// Send a GET request to a api path
        /// </summary>
        /// <param name="apiPath"> String api path</param>
        /// <returns> String with the response of the api response</returns>
        public string Get(string apiPath)
        {
            return Client.GetAsync(apiPath).Result.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Send a POST request to a api path
        /// </summary>
        /// <typeparam name="TModel"> Model that converts to the body for the api call</typeparam>
        /// <param name="apiPath"> String api path</param>
        /// <param name="model"> Model with the body data</param>
        /// <returns> String with the response of the api response</returns>
        public string Post<TModel>(string apiPath, object model)
        {
            var body = (TModel)model;

            return Client.PostAsJsonAsync(apiPath, body).Result.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Send a PUT request to a api path
        /// </summary>
        /// <typeparam name="TModel"> Model that converts to the body for the api call</typeparam>
        /// <param name="apiPath"> String api path</param>
        /// <param name="model"> Model with the body data</param>
        /// <returns> String with the response of the api response</returns>
        public string Put<TModel>(string apiPath, object model)
        {
            var body = (TModel)model;

            return Client.PutAsJsonAsync(apiPath, body).Result.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Send a DELETE request to a api path
        /// </summary>
        /// <param name="apiPath"> String api path</param>
        /// <returns> String with the response of the api response</returns>
        public string Delete(string apiPath)
        {
            return Client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, apiPath)).Result.Content.ReadAsStringAsync().Result;
        }
    }
}
