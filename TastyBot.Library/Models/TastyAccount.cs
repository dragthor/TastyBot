using Newtonsoft.Json;
using System;

namespace TastyBot.Models
{

    public class TastyAccountOverview
    {
        public TastyAccountInfo data { get; set; }
        public string context { get; set; }
    }

    public class TastyAccountInfo
    {
        public TastyAccount[] items { get; set; }
    }

    public class TastyAccount
    {
        public TastyAccountDetail account { get; set; }
        [JsonProperty(PropertyName = "authority-level")]
        public string authoritylevel { get; set; }
    }

    public class TastyAccountDetail
    {
        [JsonProperty(PropertyName = "account-number")]
        public string accountnumber { get; set; }
        [JsonProperty(PropertyName = "external-id")]
        public string externalid { get; set; }
        [JsonProperty(PropertyName = "opened-at")]
        public DateTime openedat { get; set; }
        public string nickname { get; set; }
        [JsonProperty(PropertyName = "account-type-name")]
        public string accounttypename { get; set; }
        [JsonProperty(PropertyName = "day-trader-status")]
        public bool daytraderstatus { get; set; }
        [JsonProperty(PropertyName = "is-closed")]
        public bool isclosed { get; set; }
        [JsonProperty(PropertyName = "is-firm-error")]
        public bool isfirmerror { get; set; }
        [JsonProperty(PropertyName = "is-firm-proprietary")]
        public bool isfirmproprietary { get; set; }
        [JsonProperty(PropertyName = "is-futures-approved")]
        public bool isfuturesapproved { get; set; }
        [JsonProperty(PropertyName = "is-test-drive")]
        public bool istestdrive { get; set; }
        [JsonProperty(PropertyName = "margin-or-cash")]
        public string marginorcash { get; set; }
        [JsonProperty(PropertyName = "is-foreign")]
        public bool isforeign { get; set; }
        [JsonProperty(PropertyName = "funding-date")]
        public string fundingdate { get; set; }
        [JsonProperty(PropertyName = "investment-objective")]
        public string investmentobjective { get; set; }
        [JsonProperty(PropertyName = "futures-account-purpose")]
        public string futuresaccountpurpose { get; set; }
        [JsonProperty(PropertyName = "suitable-options-level")]
        public string suitableoptionslevel { get; set; }
        [JsonProperty(PropertyName = "created-at")]
        public DateTime createdat { get; set; }
    }
}