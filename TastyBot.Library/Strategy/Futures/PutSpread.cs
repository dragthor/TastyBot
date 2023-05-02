using TastyBot.Models;
using TastyBot.Strategy;

namespace TastyBot.Library.Strategy.Futures
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

        public async Task<StrategyAttemptResult> MakeAttempt()
        {
            const int dteRange = 5;  // plus/minus days around desired DTE.
            const int qty = 1;
            const decimal otm = .90m; // 10% OTM.
            const decimal priceDrop = -2m; // Price decreased 2%.
            const decimal desiredCredit = 1.95m; // Something ridiculous so that we do NOT get filled.

            // Keep at least $1,000K in cash.  If this trade reduces our buying power below $1K, then do not submit the order.
            const decimal maintainAtLeastThisMuchBuyingPower = 1000m;

            var minDte = _daysToExpiration - dteRange;
            var maxDte = _daysToExpiration + dteRange;

            if (string.IsNullOrWhiteSpace(_ticker)) return StrategyAttemptResult.InvalidSetup;
            if (_spreadWidth < 1) return StrategyAttemptResult.InvalidSetup; // Yeah, I know some tickers have more narrow strikes.
            if (_daysToExpiration < 0) return StrategyAttemptResult.InvalidSetup;

            if (minDte < 0) minDte = _daysToExpiration;

            var anyOpenOrders = await OpenOrders();

            // Bail if we already have an open order.
            if (anyOpenOrders) return StrategyAttemptResult.NothingToDo;

            var anyExistingPositions = await ExistingPositions();

            // Bail if we already have a position.
            if (anyExistingPositions) return StrategyAttemptResult.NothingToDo;

            var quote = await _quoteMachine.getQuote(_ticker);

            var desiredStrike = Convert.ToDecimal(Math.Floor(Convert.ToDecimal(quote.price) * otm));

            if (Convert.ToDecimal(quote.dayChange) >= priceDrop)
            {
                return StrategyAttemptResult.NothingToDo;
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
                return StrategyAttemptResult.StrikeNotFound;
            }

            var shortLeg = new Leg()
            {
                instrumenttype = StrategyInstrumentType.EquityOption,
                symbol = sellStrike.put,
                action = StrategyLegAction.SellToOpen,
                quantity = qty.ToString()
            };

            var longLeg = new Leg()
            {
                instrumenttype = StrategyInstrumentType.EquityOption,
                symbol = buyStrike.put,
                action = StrategyLegAction.BuyToOpen,
                quantity = qty.ToString()
            };

            var creditSpread = CreatCreditSpread(shortLeg, longLeg, desiredCredit);

            var preview = await _bot.doDryRun(_account.account.accountnumber, creditSpread);

            if (preview.order.status.ToLower() == "received")
            {
                var newBuyingPower = Convert.ToDecimal(preview.buyingpowereffect.newbuyingpower);

                if (newBuyingPower > maintainAtLeastThisMuchBuyingPower)
                {
                    // Warning: Actually places an order when _liveOrdersEnabled is true (default is false).
                    var order = await _bot.placeOrder(_account.account.accountnumber, creditSpread);

                    // If outside normal hours, the order will be received.
                    if (order.order.status.ToLower() == "routed" || order.order.status.ToLower() == "received")
                    {
                        return StrategyAttemptResult.OrderEntered;
                    }
                    else
                    {
                        return StrategyAttemptResult.OrderRoutingError;
                    }
                }
            }
            else
            {
                if (preview.warnings.Length > 0)
                {
                    return StrategyAttemptResult.OrderWarnings;
                }

                return StrategyAttemptResult.OrderNotReceived;
            }

            return StrategyAttemptResult.NothingToDo;
        }
    }
}
