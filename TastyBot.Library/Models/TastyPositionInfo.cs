using Newtonsoft.Json;
using System;

namespace TastyBot.Models
{

    public class TastyPositionInfo
    {
        public TastyPositionData data { get; set; }
        public string context { get; set; }
    }

    public class TastyPositionData
    {
        public TastyPosition[] items { get; set; }
    }

    public class TastyPosition
    {
        [JsonProperty(PropertyName = "account-number")]
        public string accountnumber { get; set; }
        public string symbol { get; set; }
        public string instrumenttype { get; set; }
        public string underlyingsymbol { get; set; }
        public int quantity { get; set; }
        public string quantitydirection { get; set; }
        public string closeprice { get; set; }
        public string averageopenprice { get; set; }
        public string averageyearlymarketcloseprice { get; set; }
        public string averagedailymarketcloseprice { get; set; }
        public int multiplier { get; set; }
        public string costeffect { get; set; }
        public bool issuppressed { get; set; }
        public bool isfrozen { get; set; }
        public int restrictedquantity { get; set; }
        public DateTime expiresat { get; set; }
        public string realizeddaygain { get; set; }
        public string realizeddaygaineffect { get; set; }
        public string realizeddaygaindate { get; set; }
        public string realizedtoday { get; set; }
        public string realizedtodayeffect { get; set; }
        public string realizedtodaydate { get; set; }
        public DateTime createdat { get; set; }
        public DateTime updatedat { get; set; }
    }
}