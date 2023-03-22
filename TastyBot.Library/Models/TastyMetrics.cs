using Newtonsoft.Json;
using System;

namespace TastyBot.Models
{

    public class TastyMetricsInfo
    {
        public TastyMetrics data { get; set; }
    }

    public class TastyMetrics
    {
        public TastyMetricsItem[] items { get; set; }
    }

    public class TastyMetricsItem
    {
        [JsonProperty(PropertyName = "symbol")]
        public string symbol { get; set; }

        [JsonProperty(PropertyName = "implied-volatility-index")]
        public string impliedvolatilityindex { get; set; }

        [JsonProperty(PropertyName = "implied-volatility-index-5-day-change")]
        public string impliedvolatilityindex5daychange { get; set; }

        [JsonProperty(PropertyName = "implied-volatility-index-rank")]
        public string impliedvolatilityindexrank { get; set; }

        [JsonProperty(PropertyName = "tos-implied-volatility-index-rank")]
        public string tosimpliedvolatilityindexrank { get; set; }

        [JsonProperty(PropertyName = "tw-implied-volatility-index-rank")]
        public string twimpliedvolatilityindexrank { get; set; }

        [JsonProperty(PropertyName = "tos-implied-volatility-index-rank-updated-at")]
        public DateTime tosimpliedvolatilityindexrankupdatedat { get; set; }

        [JsonProperty(PropertyName = "implied-volatility-index-rank-source")]
        public string impliedvolatilityindexranksource { get; set; }

        [JsonProperty(PropertyName = "implied-volatility-percentile")]
        public string impliedvolatilitypercentile { get; set; }

        [JsonProperty(PropertyName = "implied-volatility-updated-at")]
        public DateTime impliedvolatilityupdatedat { get; set; }

        [JsonProperty(PropertyName = "liquidity-value")]
        public string liquidityvalue { get; set; }

        [JsonProperty(PropertyName = "liquidity-rank")]
        public string liquidityrank { get; set; }

        [JsonProperty(PropertyName = "liquidity-rating")]
        public int liquidityrating { get; set; }

        [JsonProperty(PropertyName = "updated-at")]
        public DateTime updatedat { get; set; }

        [JsonProperty(PropertyName = "option-expiration-implied-volatilities")]
        public List<OptionExpirationImpliedVolatility> optionexpirationimpliedvolatilities { get; set; }

        [JsonProperty(PropertyName = "liquidity-running-state")]
        public LiquidityRunningState liquidityrunningstate { get; set; }

        // Add the remaining properties
        [JsonProperty(PropertyName = "beta")]
        public string beta { get; set; }

        [JsonProperty(PropertyName = "corr-spy-3month")]
        public string corrspy3month { get; set; }

        [JsonProperty(PropertyName = "dividend-rate-per-share")]
        public string dividendratepershare { get; set; }

        [JsonProperty(PropertyName = "annual-dividend-per-share")]
        public string annualdividendpershare { get; set; }

        [JsonProperty(PropertyName = "dividend-yield")]
        public string dividyield { get; set; }

        [JsonProperty(PropertyName = "dividend-ex-date")]
        public string dividendexdate { get; set; }

        [JsonProperty(PropertyName = "dividend-next-date")]
        public string dividendnextdate { get; set; }

        [JsonProperty(PropertyName = "dividend-pay-date")]
        public string dividendpaydate { get; set; }

        [JsonProperty(PropertyName = "dividend-updated-at")]
        public DateTime dividendupdatedat { get; set; }

        [JsonProperty(PropertyName = "earnings")]
        public Earnings earnings { get; set; }

        [JsonProperty(PropertyName = "listed-market")]
        public string listedmarket { get; set; }

        [JsonProperty(PropertyName = "lendability")]
        public string lendability { get; set; }

        [JsonProperty(PropertyName = "borrow-rate")]
        public string borrowrate { get; set; }

        [JsonProperty(PropertyName = "market-cap")]
        public int marketcap { get; set; }

        [JsonProperty(PropertyName = "implied-volatility-30-day")]
        public string impliedvolatility30day { get; set; }

        [JsonProperty(PropertyName = "historical-volatility-30-day")]
        public string historicalvolatility30day { get; set; }

        [JsonProperty(PropertyName = "historical-volatility-60-day")]
        public string historicalvolatility60day { get; set; }

        [JsonProperty(PropertyName = "historical-volatility-90-day")]
        public string historicalvolatility90day { get; set; }

        [JsonProperty(PropertyName = "iv-hv-30-day-difference")]
        public string ivhv30daydifference { get; set; }
    }

   public class OptionExpirationImpliedVolatility
    {
        [JsonProperty(PropertyName = "expiration-date")]
        public string expirationdate { get; set; }

        [JsonProperty(PropertyName = "option-chain-type")]
        public string optionchaintype { get; set; }

        [JsonProperty(PropertyName = "settlement-type")]
        public string settlementtype { get; set; }

        [JsonProperty(PropertyName = "implied-volatility")]
        public string impliedvolatility { get; set; }
    }

    public class LiquidityRunningState
    {
        [JsonProperty(PropertyName = "sum")]
        public string sum { get; set; }

        [JsonProperty(PropertyName = "count")]
        public int count { get; set; }

        [JsonProperty(PropertyName = "started-at")]
        public DateTime startedat { get; set; }

        [JsonProperty(PropertyName = "updated-at")]
        public DateTime updatedat { get; set; }
    }

    public class Earnings
    {
        [JsonProperty(PropertyName = "visible")]
        public bool visible { get; set; }

        [JsonProperty(PropertyName = "estimated")]
        public bool estimated { get; set; }

        [JsonProperty(PropertyName = "late-flag")]
        public int lateflag { get; set; }
    }
}