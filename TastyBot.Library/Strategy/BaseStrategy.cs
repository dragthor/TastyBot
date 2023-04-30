using TastyBot.Library;
using TastyBot.Models;

namespace TastyBot.Strategy
{
    public enum StrategyAttemptResult
    {
        NothingToDo,
        OrderEntered,
        StrikeNotFound,
        InvalidSetup,
        OrderRoutingError,
        OrderWarnings,
        OrderNotReceived
    }

    public static class StrategyOrderResultType
    {
        public const string Credit = "Credit";
        public const string Debit = "Debit";
    }

    public static class StrategyLegAction
    {
        public const string BuyToOpen = "Buy to Open";
        public const string SellToOpen = "Sell to Open";
    }

    public static class StrategyInstrumentType
    {
        public const string EquityOption = "Equity Option";
    }

    public static class StrategyOrderInForce
    {
        public const string Day = "Day";
    }

    public static class StrategyOrderType
    {
        public const string Limit = "Limit";
    }

    public static class StrategyOrderSource
    {
        public const string Name = "WBT-ember;";
        // public const string Name = "TastyBot;";
    }

    public abstract class BaseStrategy
    {
        protected readonly ITastyBot _bot;
        protected readonly IQuoteMachine _quoteMachine;
        protected readonly TastyAccount _account;
        protected readonly string _ticker;

        public BaseStrategy(ITastyBot bot, IQuoteMachine quoteMachine, TastyAccount account, string ticker)
        {
            _bot = bot;
            _quoteMachine = quoteMachine;
            _account = account;
            _ticker = ticker;
        }

        public async Task<bool> OpenOrders()
        {
            var orders = await _bot.getOrders(_account.account.accountnumber);

            var openOrders = orders.items.Count(x => x.underlyingsymbol == _ticker && (x.status.ToLower() == "live" || x.status.ToLower() == "received"));

            return openOrders > 0;
        }

        public async Task<bool> ExistingPositions()
        {
            var positions = await _bot.getPositions(_account.account.accountnumber);

            var existingPositions = positions.items.Count(x => x.symbol == _ticker);

            return existingPositions > 0;
        }
    }
}
