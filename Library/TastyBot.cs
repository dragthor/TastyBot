using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TastyBot.Models;
using System.Collections.Generic;
using TastyBot.Rules;

namespace TastyBot.Library
{
    public interface ITastyBot
    {
        Task<TastySessionInfo> getAuthorization();
        Task<TastyAccountInfo> getAccounts();
        Task<TastyBalance> getBalance(string accountId);
        Task<TastyMetrics> getMarketMetrics(string tickers);
        Task<TastyChain> getOptionChain(string ticker);
        Task<TastyStreamer> getStreamerTokens();
        Task<TastyOrderData> doDryRun(string accountId, TastySpread spread);
        Task<TastyOrderData> placeOrder(string accountId, TastySpread spread);
        Task<TastyPositionData> getPositions(string accountId);
        Task<TastyLiveOrderData> getOrders(string accountId);
        Task<List<RuleResult>> processRules();
        void Terminate();
    }

    public class TastyBot : ITastyBot
    {
        private readonly string _secretName;
        private readonly string _secretSauce;
        private readonly string _baseUrl;
        private readonly int _timeOut;

        private readonly HttpClient _client;

        private string _authToken;
        private bool _liveOrdersEnabled = false;

        private TastyBot(string secretName, string secretSauce, string baseUrl, int timeOut) {
            _secretName = secretName;
            _secretSauce= secretSauce;
            _baseUrl = baseUrl;
            _timeOut = timeOut;

            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli
            };

            _client = new HttpClient(handler);
            _client.Timeout = TimeSpan.FromSeconds(_timeOut);
        }


        public static ITastyBot CreateInstance(string secretName, string secretSauce, string baseUrl, int timeOut)
        {
            var bot = new TastyBot(secretName, secretSauce, baseUrl, timeOut);

            return bot;
        }

        public async Task<TastySessionInfo> getAuthorization()
        {
            var creds = new TastyCredentials() { login = _secretName, password = _secretSauce };
            var json = JsonConvert.SerializeObject(creds);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = getRequest("/sessions", HttpMethod.Post, content);

            var response = await _client.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<TastySession>(result);

            _authToken = obj.data.sessiontoken;

            return obj.data;
        }

        public async Task<TastyAccountInfo> getAccounts()
        {
            var request = getRequest("/customers/me/accounts", HttpMethod.Get);

            var response = await _client.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<TastyAccountOverview>(result);

            return obj.data;
        }

        public async Task<TastyBalance> getBalance(string accountId)
        {
            var request = getRequest($"/accounts/{accountId}/balances", HttpMethod.Get);

            var response = await _client.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<TastyBalanceInfo>(result);

            return obj.data;
        }

        public async Task<TastyMetrics> getMarketMetrics(string ticker)
        {
            var request = getRequest($"/market-metrics?symbols={ticker}", HttpMethod.Get);

            var response = await _client.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<TastyMetricsInfo>(result);

            return obj.data;
        }

        public async Task<TastyChain> getOptionChain(string ticker)
        {
            var request = getRequest($"/option-chains/{ticker}/nested", HttpMethod.Get);

            var response = await _client.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<TastyChainInfo>(result);

            return obj.data;
        }

        public async Task<TastyStreamer> getStreamerTokens()
        {
            var request = getRequest("/quote-streamer-tokens", HttpMethod.Get);

            var response = await _client.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<TastyStreamerInfo>(result);

            return obj.data;
        }

        public async Task<TastyOrderData> doDryRun(string accountId, TastySpread spread)
        {
            var json = JsonConvert.SerializeObject(spread);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = getRequest($"/accounts/{accountId}/orders/dry-run", HttpMethod.Post, content);

            var response = await _client.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<TastyOrderInfo>(result);

            return obj.data;
        }

        public async Task<TastyOrderData> placeOrder(string accountId, TastySpread spread)
        {
            // Fail safe.
            if (_liveOrdersEnabled == false) throw new Exception("Live orders are NOT enabled.");

            var json = JsonConvert.SerializeObject(spread);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = getRequest($"/accounts/{accountId}/orders", HttpMethod.Post, content);

            var response = await _client.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<TastyOrderInfo>(result);

            return obj.data;
        }

        public async Task<TastyPositionData> getPositions(string accountId)
        {
            var request = getRequest($"/accounts/{accountId}/positions", HttpMethod.Get);

            var response = await _client.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<TastyPositionInfo>(result);

            return obj.data;
        }

        public async Task<TastyLiveOrderData> getOrders(string accountId)
        {
            var request = getRequest($"/accounts/{accountId}/orders/live", HttpMethod.Get);

            var response = await _client.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<TastyLiveOrderInfo>(result);

            return obj.data;
        }

        public void Terminate()
        {
            if (_client != null)
            {
                _client.Dispose();
            }
        }

        private HttpRequestMessage getRequest(string action, HttpMethod method, StringContent? content = null)
        {
            var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri($"{_baseUrl}{action}")
            };

            if (content != null)
            {
                request.Content = content;
            }

            request.Headers.Add("Accept", "application/json, text/javascript, */*; q=0.01");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            request.Headers.Add("Referer", "https://trade.tastyworks.com/");

            request.Headers.Add("cache-control", "no-cache");
            request.Headers.Add("pragma", "no-cache");
            request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.36");
            request.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");

            if (!string.IsNullOrEmpty(_authToken))
            {
                request.Headers.Add("authorization", _authToken);
            }

            return request;
        }

        public async Task<List<RuleResult>> processRules()
        {
            // TODO: Implement the following example rules:
            //
            // Do I have sufficient buying power?
            // Is the VIX above 20?
            // Did SPX drop 2%?
            var list = new List<RuleResult>();

            return list;
        }
    }
}