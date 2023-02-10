using Newtonsoft.Json;

namespace TastyBot.Models
{
    public class TastySpread
    {        
        public string source { get; set; }
        [JsonProperty(PropertyName = "order-type")]
        public string ordertype { get; set; }
        [JsonProperty(PropertyName = "time-in-force")]
        public string timeinforce { get; set; }
        public string price { get; set; }
        [JsonProperty(PropertyName = "price-effect")]
        public string priceeffect { get; set; }
        public Leg[] legs { get; set; }
    }

    public class Leg
    {
        [JsonProperty(PropertyName = "instrument-type")]
        public string instrumenttype { get; set; }
        public string symbol { get; set; }
        public string action { get; set; }
        public string quantity { get; set; }
    }
}