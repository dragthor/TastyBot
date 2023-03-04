using Newtonsoft.Json;

namespace TastyBot.Models
{

    public class TastyChainInfo
    {
        public TastyChain data { get; set; }
        public string context { get; set; }
    }

    public class TastyChain
    {
        public TastyChainItem[] items { get; set; }
    }

    public class TastyChainItem
    {
        [JsonProperty(PropertyName = "underlying-symbol")]
        public string underlyingsymbol { get; set; }
        [JsonProperty(PropertyName = "root-symbol")]
        public string rootsymbol { get; set; }
        public string optionchaintype { get; set; }
        public int sharespercontract { get; set; }
        public TickSizes[] ticksizes { get; set; }
        public Deliverable[] deliverables { get; set; }
        public Expiration[] expirations { get; set; }
    }

    public class TickSizes
    {
        public string value { get; set; }
        public string threshold { get; set; }
    }

    public class Deliverable
    {
        public int id { get; set; }
        [JsonProperty(PropertyName = "root-symbol")]
        public string rootsymbol { get; set; }
        public string deliverabletype { get; set; }
        public string description { get; set; }
        public string amount { get; set; }
        public string percent { get; set; }
    }

    public class Expiration
    {
        [JsonProperty(PropertyName = "expiration-type")]
        public string expirationtype { get; set; }
        [JsonProperty(PropertyName = "expiration-date")]
        public string expirationdate { get; set; }
        [JsonProperty(PropertyName = "days-to-expiration")]
        public int daystoexpiration { get; set; }
        public string settlementtype { get; set; }
        public Strike[] strikes { get; set; }

        public override string ToString()
        {
            return daystoexpiration.ToString();
        }
    }

    public class Strike
    {
        [JsonProperty(PropertyName = "strike-price")]
        public string strikeprice { get; set; }
        public string call { get; set; }
        public string put { get; set; }

        public override string ToString()
        {
            return strikeprice ?? "0";
        }
    }
}