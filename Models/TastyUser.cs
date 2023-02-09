using Newtonsoft.Json;

namespace TastyBot.Models
{
    public class TastyUser
    {
        public string email { get; set; }
        public string username { get; set; }
        [JsonProperty(PropertyName = "external-id")]
        public string externalid { get; set; }
    }
}