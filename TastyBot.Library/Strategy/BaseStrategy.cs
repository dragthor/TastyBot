using TastyBot.Library;
using TastyBot.Library.ThirdParty;
using TastyBot.Models;

namespace TastyBot.Strategy
{
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

            var openOrders = orders.items.Count(x => x.underlyingsymbol == _ticker && x.status.ToLower() == "live");

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
