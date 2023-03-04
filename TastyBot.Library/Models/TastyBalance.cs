using Newtonsoft.Json;
using System;

namespace TastyBot.Models
{
    public class TastyBalanceInfo
    {
        public TastyBalance data { get; set; }
        public string context { get; set; }
    }

    public class TastyBalance
    {
        [JsonProperty(PropertyName = "account-number")]
        public string accountnumber { get; set; }
        [JsonProperty(PropertyName = "cash-balance")]
        public string cashbalance { get; set; }
        public string longequityvalue { get; set; }
        public string shortequityvalue { get; set; }
        public string longderivativevalue { get; set; }
        public string shortderivativevalue { get; set; }
        public string longfuturesvalue { get; set; }
        public string shortfuturesvalue { get; set; }
        public string longfuturesderivativevalue { get; set; }
        public string shortfuturesderivativevalue { get; set; }
        public string longmargineablevalue { get; set; }
        public string shortmargineablevalue { get; set; }
        [JsonProperty(PropertyName = "margin-equity")]
        public string marginequity { get; set; }
        [JsonProperty(PropertyName = "equity-buying-power")]
        public string equitybuyingpower { get; set; }
        [JsonProperty(PropertyName = "derivative-buying-power")]
        public string derivativebuyingpower { get; set; }
        [JsonProperty(PropertyName = "day-trading-buying-power")]
        public string daytradingbuyingpower { get; set; }
        [JsonProperty(PropertyName = "futures-margin-requirement")]
        public string futuresmarginrequirement { get; set; }
        public string availabletradingfunds { get; set; }
        public string maintenancerequirement { get; set; }
        public string maintenancecallvalue { get; set; }
        public string regtcallvalue { get; set; }
        public string daytradingcallvalue { get; set; }
        public string dayequitycallvalue { get; set; }
        public string netliquidatingvalue { get; set; }
        public string cashavailabletowithdraw { get; set; }
        public string daytradeexcess { get; set; }
        public string pendingcash { get; set; }
        public string pendingcasheffect { get; set; }
        public string longcryptocurrencyvalue { get; set; }
        public string shortcryptocurrencyvalue { get; set; }
        public string cryptocurrencymarginrequirement { get; set; }
        public string unsettledcryptocurrencyfiatamount { get; set; }
        public string unsettledcryptocurrencyfiateffect { get; set; }
        public string closedloopavailablebalance { get; set; }
        public string equityofferingmarginrequirement { get; set; }
        public string longbondvalue { get; set; }
        public string bondmarginrequirement { get; set; }
        public string snapshotdate { get; set; }
        [JsonProperty(PropertyName = "reg-t-margin-requirement")]
        public string regtmarginrequirement { get; set; }
        public string futuresovernightmarginrequirement { get; set; }
        public string futuresintradaymarginrequirement { get; set; }
        [JsonProperty(PropertyName = "maintenance-excess")]
        public string maintenanceexcess { get; set; }
        public string pendingmargininterest { get; set; }
        public string effectivecryptocurrencybuyingpower { get; set; }
        public DateTime updatedat { get; set; }
    }
}