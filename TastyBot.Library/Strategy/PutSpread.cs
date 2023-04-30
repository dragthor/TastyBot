using TastyBot.Library;
using TastyBot.Library.ThirdParty;
using TastyBot.Models;

namespace TastyBot.Strategy
{
    public class PutSpread : BaseStrategy, ITastyBotStrategy
    {
        private readonly int _spreadWidth;
        private readonly int _daysToExpiration;

        public PutSpread(ITastyBot bot, IQuoteMachine quoteMachine, TastyAccount account, string ticker, int spreadWidth, int daysToExpiration) : base(bot, quoteMachine, account, ticker)
        {
            _spreadWidth = spreadWidth;
            _daysToExpiration = daysToExpiration;
        }

        public static ITastyBotStrategy CreateInstance(ITastyBot bot, IQuoteMachine quoteMachine, TastyAccount account, string ticker, int spreadWidth, int daysToExpiration)
        {
            return new PutSpread(bot, quoteMachine, account, ticker, spreadWidth, daysToExpiration);
        }

        public async Task<int> MakeAttempt()
        {
            const int dteRange = 5;  // plus/minus days around desired DTE.

            var minDte = _daysToExpiration - dteRange;
            var maxDte = _daysToExpiration + dteRange;

            var anyOpenOrders = await OpenOrders();

            // Bail if we already have an open order.
            if (anyOpenOrders) return 0;

            var anyExistingPositions = await ExistingPositions();

            // Bail if we already have a position.
            if (anyExistingPositions) return 0;

            var quote = await _quoteMachine.getQuote(_ticker);

            // 10% OTM.
            var desiredStrike = Convert.ToDecimal(Math.Floor(quote.price * .90));

            // Keep at least $2,500K in cash.
            var maintainAtLeastThisMuchBuyingPower = 2500;

            // Bail if we do not have a 2% drop.
            if (quote.dayChange > -2d)
            {
                return 0;
            }

            var optionChain = await _bot.getOptionChain(_ticker);

            Strike? sellStrike = null;
            Strike? buyStrike = null;

            // Just look at the monthlies (due to SPX vs SPXW).
            foreach (var chain in optionChain.items.ToList().Where(x => x.rootsymbol == _ticker))
            {
                if (sellStrike != null || buyStrike != null) break;

                var expirations = chain.expirations.Where(x => x.daystoexpiration >= minDte && x.daystoexpiration <= maxDte).ToList().OrderByDescending(x => x.daystoexpiration);

                foreach (var expiration in expirations)
                {
                    var strikes = expiration.strikes.ToList();

                    sellStrike = strikes.Where(x => Convert.ToDecimal(x.strikeprice) == desiredStrike).FirstOrDefault();
                    buyStrike = strikes.Where(x => Convert.ToDecimal(x.strikeprice) == desiredStrike - _spreadWidth).FirstOrDefault();

                    if (sellStrike != null && buyStrike != null)
                    {
                        // Bingo.
                        break;
                    }
                }
            }

            // Double check.
            if (sellStrike == null || buyStrike == null)
            {
                return 0;
            }

            var shortLeg = new Leg()
            {
                instrumenttype = "Equity Option",
                symbol = sellStrike.put,
                action = "Sell to Open",
                quantity = "1"
            };

            var longLeg = new Leg()
            {
                instrumenttype = "Equity Option",
                symbol = buyStrike.put,
                action = "Buy to Open",
                quantity = "1"
            };

            var creditSpread = new TastySpread()
            {
                source = "WBT-ember;",
                ordertype = "Limit",
                timeinforce = "Day",
                price = "1.95", // Something ridiculous so that we do NOT get filled.
                priceeffect = "Credit",
                legs = new Leg[] { shortLeg, longLeg }
            };

            var preview = await _bot.doDryRun(_account.account.accountnumber, creditSpread);

            if (preview.warnings.Length == 0)
            {
                if (preview.order.status.ToLower() == "received")
                {
                    var newBuyingPower = Convert.ToDecimal(preview.buyingpowereffect.newbuyingpower);

                    if (newBuyingPower > maintainAtLeastThisMuchBuyingPower)
                    {
                        // Warning: Actually places an order when _liveOrdersEnabled is true (default is false).
                        var order = await _bot.placeOrder(_account.account.accountnumber, creditSpread);

                        if (order.order.status.ToLower() == "routed")
                        {
                            return 1;
                        }
                    }
                }
            }

            return 0;
        }
    }
}