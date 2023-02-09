using Newtonsoft.Json;

namespace TastyBot.Models
{
    public class TastySessionInfo
    {
        public TastyUser user { get; set; }
        [JsonProperty(PropertyName = "session-token")]
        public string sessiontoken { get; set; }
    }
}