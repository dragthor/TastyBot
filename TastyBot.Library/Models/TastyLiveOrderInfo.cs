using Newtonsoft.Json;
using System;

namespace TastyBot.Models
{

    public class TastyLiveOrderInfo
    {
        public TastyLiveOrderData data { get; set; }
        public string context { get; set; }
    }

    public class TastyLiveOrderData
    {
        public TastyLiveOrder[] items { get; set; }
    }

    public class TastyLiveOrder
    {
        public int id { get; set; }
        [JsonProperty(PropertyName = "account-number")]
        public string accountnumber { get; set; }
        public string timeinforce { get; set; }
        public string ordertype { get; set; }
        public int size { get; set; }
        [JsonProperty(PropertyName = "underlying-symbol")]
        public string underlyingsymbol { get; set; }
        public string underlyinginstrumenttype { get; set; }
        public string price { get; set; }
        public string priceeffect { get; set; }
        public string status { get; set; }
        public bool cancellable { get; set; }
        public bool editable { get; set; }
        public bool edited { get; set; }
        public string extexchangeordernumber { get; set; }
        public string extclientorderid { get; set; }
        public int extglobalordernumber { get; set; }
        public DateTime receivedat { get; set; }
        public long updatedat { get; set; }
        public Leg[] legs { get; set; }
        public string rejectreason { get; set; }
        public DateTime terminalat { get; set; }
    }
}
