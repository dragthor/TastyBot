using Newtonsoft.Json;
using System.Net;
using TastyBot.Rules;

namespace TastyBot.Library.ThirdParty
{
    // https://www.stockdata.org
    public class StockDataOrg : IQuoteMachine
    {
        private readonly string _baseQuoteUrl;
        private readonly int _timeOut;
        private readonly string _apiToken;

        private readonly HttpClient _client;

        private StockDataOrg(string baseQuoteUrl, int timeOut, string apiToken)
        {
            _baseQuoteUrl = baseQuoteUrl;
            _timeOut = timeOut;
            _apiToken = apiToken;

            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli
            };

            _client = new HttpClient(handler);
            _client.Timeout = TimeSpan.FromSeconds(_timeOut);
        }

        public static IQuoteMachine CreateInstance(string baseQuoteUrl, int timeOut, string apiToken)
        {
            return new StockDataOrg(baseQuoteUrl, timeOut, apiToken);
        }

        public async Task<IRuleQuote> getQuote(string ticker)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseQuoteUrl}/v1/data/quote?symbols={ticker}&api_token={_apiToken}")
            };

            var response = await _client.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<StockDataQuoteInfo>(result);

            return obj.data.First();
        }

        public Task Terminate()
        {
            if (_client != null)
            {
                _client.Dispose();
            }

            return Task.CompletedTask;
        }
    }
}