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
        public string symbol { get; set; }
        public string impliedvolatilityindex { get; set; }
        public string impliedvolatilityindex5daychange { get; set; }
        [JsonProperty(PropertyName = "implied-volatility-index-rank")]
        public string impliedvolatilityindexrank { get; set; }
        [JsonProperty(PropertyName = "tos-implied-volatility-index-rank")]
        public string tosimpliedvolatilityindexrank { get; set; }
        [JsonProperty(PropertyName = "tw-implied-volatility-index-rank")]
        public string twimpliedvolatilityindexrank { get; set; }
        public DateTime tosimpliedvolatilityindexrankupdatedat { get; set; }
        public string impliedvolatilityindexranksource { get; set; }
        public string impliedvolatilitypercentile { get; set; }
        public DateTime impliedvolatilityupdatedat { get; set; }
        [JsonProperty(PropertyName = "liquidity-value")]
        public string liquidityvalue { get; set; }
        [JsonProperty(PropertyName = "liquidity-rank")]
        public string liquidityrank { get; set; }
        [JsonProperty(PropertyName = "liquidity-rating")]
        public int liquidityrating { get; set; }
        public DateTime updatedat { get; set; }
        public OptionExpirationImpliedVolatilities[] optionexpirationimpliedvolatilities { get; set; }
        public LiquidityRunningState liquidityrunningstate { get; set; }
        public string beta { get; set; }
        public string corrspy3month { get; set; }
        public string dividendratepershare { get; set; }
        public string annualdividendpershare { get; set; }
        public string dividendyield { get; set; }
        public string dividendexdate { get; set; }
        public string dividendnextdate { get; set; }
        public string dividendpaydate { get; set; }
        public int marketcap { get; set; }
        public string impliedvolatility30day { get; set; }
        public string historicalvolatility30day { get; set; }
        public string historicalvolatility60day { get; set; }
        public string historicalvolatility90day { get; set; }
        public string ivhv30daydifference { get; set; }
    }

    public class LiquidityRunningState
    {
        public string sum { get; set; }
        public int count { get; set; }
        public DateTime startedat { get; set; }
        public DateTime updatedat { get; set; }
    }

    public class OptionExpirationImpliedVolatilities
    {
        public string expirationdate { get; set; }
        public string optionchaintype { get; set; }
        public string settlementtype { get; set; }
        public string impliedvolatility { get; set; }
    }
}