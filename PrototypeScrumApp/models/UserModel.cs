using Newtonsoft.Json;

namespace PrototypeScrumApp.models
{
    public class UserModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("invite_code")]
        public string InviteCode { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("password_confirmation")]
        public string RepeatPassword { get; set; }
 
        [JsonProperty("api_token")]
        public string ApiToken { get; set; }
        
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        public bool CheckBoxChecked { get; set; }
    }
}
