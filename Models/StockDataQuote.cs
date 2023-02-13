using System;
using TastyBot.Rules;

namespace TastyBot.Models
{
    public class StockDataQuoteInfo
    {
        public StockDataQuoteMeta meta { get; set; }
        public StockDataQuote[] data { get; set; }
    }

    public class StockDataQuoteMeta
    {
        public int requested { get; set; }
        public int returned { get; set; }
    }

    public class StockDataQuote : IRuleQuote
    {
        public string ticker { get; set; }
        public string name { get; set; }
        public string exchange_short { get; set; }
        public string exchange_long { get; set; }
        public string mic_code { get; set; }
        public string currency { get; set; }
        public float price { get; set; }
        public float day_high { get; set; }
        public float day_low { get; set; }
        public float day_open { get; set; }
        public float _52_week_high { get; set; }
        public float _52_week_low { get; set; }
        public object market_cap { get; set; }
        public float previous_close_price { get; set; }
        public DateTime previous_close_price_time { get; set; }
        public float day_change { get; set; }
        public int volume { get; set; }
        public bool is_extended_hours_price { get; set; }
        public DateTime last_trade_time { get; set; }

        public float dayChange
        {
            get
            {
                return day_change;
            }
        }
    }
}