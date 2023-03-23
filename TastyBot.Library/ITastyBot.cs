using TastyBot.Models;
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
        Task Terminate();
    }
}