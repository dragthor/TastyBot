using Newtonsoft.Json;

namespace TastyBot.Models
{

    public class TastyStreamerInfo
    {
        public TastyStreamer data { get; set; }
        public string context { get; set; }
    }

    public class TastyStreamer
    {
        public string token { get; set; }
        [JsonProperty(PropertyName = "streamer-url")]
        public string streamerurl { get; set; }
        [JsonProperty(PropertyName = "websocket-url")]
        public string websocketurl { get; set; }
        public string level { get; set; }
    }
}