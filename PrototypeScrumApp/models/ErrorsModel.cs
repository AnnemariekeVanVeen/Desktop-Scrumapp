using Newtonsoft.Json;
using System.Collections.Generic;

namespace PrototypeScrumApp.models
{
    internal class ErrorsModel
    {
        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("Reason")]
        public string Reason { get; set; }

        [JsonProperty("Errors")]
        public List<Error> Errors { get; set; }
    }

    internal class Error
    {
        public string ErrorString { get; set; }
    }
}
