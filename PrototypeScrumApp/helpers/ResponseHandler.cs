using Newtonsoft.Json.Linq;
using PrototypeScrumApp.models;
using System.Collections.Generic;
using System.Windows;

namespace PrototypeScrumApp.helpers
{
    internal class ResponseHandler
    {
        // TODO: finish error handling from api
        public ErrorsModel HandleErrorResponses(string response)
        {
            // Make json object of string
            var parse = JObject.Parse(response);

            // Make json array of json obj
            var message = parse["Message"];

            return new ErrorsModel();
        }

        /// <summary>
        /// Checks of the response has a OK in the message
        /// </summary>
        /// <param name="response">Response string of the api call</param>
        /// <returns>Bool, true when response contains OK message. false when something else</returns>
        public bool CheckForErrorResponse(string response)
        {
            return ParseToToken(response, "Message").ToString() == "OK";
        }

        /// <summary>
        /// Checks if the non capital message is send
        /// Checks of the response has a OK in the message
        /// </summary>
        /// <param name="response">Response string of the api call</param>
        /// <returns>Bool, true when response contains OK message. false when something else</returns>
        public bool CheckForErrorResponseProject(string response)
        {
            var parse = JObject.Parse(response);
            var a = parse["message"];
            if (a != null)
            {
                return false;
            }
            return ParseToToken(response, "Message").ToString() == "OK";
        }

        /// <summary>
        /// Dynamic format response to a model
        /// </summary>
        /// <typeparam name="TModel">Model to format to</typeparam>
        /// <param name="response">Response string of the api</param>
        /// <param name="jsonKey">Key of the json response where the model needs to format to</param>
        /// <returns>the model from your type param</returns>
        public TModel SingleObjectResponse<TModel>(string response, string jsonKey)
        {
            return ParseToToken(response, jsonKey).ToObject<TModel>();
        }

        /// <summary>
        /// Dynamic format response to a IList of models 
        /// </summary>
        /// <typeparam name="TModel">Model to format to</typeparam>
        /// <param name="response">Response string of api</param>
        /// <param name="jsonKey">Key of the json response where the model needs to be format to</param>
        /// <returns>IList of models from your type param</returns>
        public IList<TModel> ListOfObjectsResponse<TModel>(string response, string jsonKey)
        {
            return ParseToArray(response, jsonKey).ToObject<IList<TModel>>();
        }

        /// <summary>
        /// Dynamic format response to a IList of models 
        /// </summary>
        /// <typeparam name="TModel">Model to format to</typeparam>
        /// <param name="response">Response string of api</param>
        /// <param name="jsonKey1">Key of the json response where the model needs to be format to</param>
        /// <param name="jsonKey2">Key of the json response where the model needs to be format to</param>
        /// <returns>IList of models from your type param</returns>
        public IList<TModel> ListOfObjects<TModel>(string response, string jsonKey1, string jsonKey2)
        {
            var parse = JObject.Parse(response);
            var object1 = parse[jsonKey1];
            var array2 = (JArray)object1[jsonKey2];
            return array2.ToObject<IList<TModel>>();
        }

        /// <summary>
        /// Parse response to JArray
        /// </summary>
        /// <param name="response">Response string from a api</param>
        /// <param name="jsonKey">Json key of a item you want the JArray from</param>
        /// <returns>JArray of the response</returns>
        private static JArray ParseToArray(string response, string jsonKey)
        {
            var parse = JObject.Parse(response);

            return (JArray)parse[jsonKey];
        }

        /// <summary>
        /// Parse response to a JToken
        /// </summary>
        /// <param name="response">Response string from a api</param>
        /// <param name="jsonKey">Json key of what item you want to parse</param>
        /// <returns>JToken of the response</returns>
        private static JToken ParseToToken(string response, string jsonKey)
        {
            var parse = JObject.Parse(response);

            return parse[jsonKey];
        }
    }
}
