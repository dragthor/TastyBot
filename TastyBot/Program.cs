using System;
using System.Threading.Tasks;
using System.Linq;
using TastyBot.Models;

namespace TastyBot
{
    public partial class Program
    {
        private const string BaseUrl = "https://api.tastyworks.com";
        private const string BaseQuoteUrl = "https://api.stockdata.org";
        private const int TimeOut = 10;
        
        public static async Task Main(string[] args)
        {
            var tastyBot = Library.TastyBot.CreateDebugInstance(SecretName, SecretSauce, BaseUrl, TimeOut);
            var quoteMachine = Library.StockDataOrg.CreateInstance(BaseQuoteUrl, TimeOut, StockDataOrgApiToken);

            try
            {
                var sessionInfo = await tastyBot.getAuthorization();

                Console.WriteLine("Welcome: " + sessionInfo.user.username);

                var accounts = await tastyBot.getAccounts();

                accounts.items.ToList().ForEach(account =>
                {
                    Console.WriteLine("Account: " + account.account.accountnumber);
                });

                var primaryAccount = accounts.items.First();
                var balance = await tastyBot.getBalance(primaryAccount.account.accountnumber);
                var positions = await tastyBot.getPositions(primaryAccount.account.accountnumber);
                var orders = await tastyBot.getOrders(primaryAccount.account.accountnumber);

                Console.WriteLine("Cash: " + balance.cashbalance + ", Maintenance: " + balance.maintenanceexcess + ", Reg-T Margin: " + balance.regtmarginrequirement + ", Futures Margin; " + balance.futuresmarginrequirement);

                var ticker = "SPY";
                var quote = await quoteMachine.getQuote(ticker);
                var metrics = await tastyBot.getMarketMetrics(ticker);
                var chains = await tastyBot.getOptionChain(ticker);
                
                // 10% OTM.
                var desiredStrike = Convert.ToDecimal(Math.Floor(quote.price * .90));

                // $500 max risk.
                var desiredRisk = Convert.ToDecimal(500 / 100);
                
                // Keep at least $2,500K in cash.
                var maintainAtLeastThisMuchBuyingPower = 2500;

                var openOrders = orders.items.Count(x => x.underlyingsymbol == ticker && x.status == "Live");
                var existingPositions = positions.items.Count(x => x.symbol == ticker);

                // Bail if we do not have a 2% drop.
                if (quote.dayChange > -2d)
                {
                    Console.WriteLine("2% decrease not reached.");
                    return;
                }

                // Bail if we already have an open order.
                if (openOrders > 0)
                {
                    Console.WriteLine("Already have an open order.");
                    return;
                }

                // Bail if we already have a position.
                if (existingPositions > 0)
                {
                    Console.WriteLine("Position limit reached.");
                    return;
                }

                Strike? sellStrike = null;
                Strike? buyStrike = null;

                // Just look at the monthlies (due to SPX vs SPXW).
                foreach (var chain in chains.items.ToList().Where(x => x.rootsymbol == ticker))
                {
                    if (sellStrike != null || buyStrike != null) break;

                    var expirations = chain.expirations.Where(x => x.daystoexpiration >= 30 && x.daystoexpiration <= 50).ToList().OrderByDescending(x => x.daystoexpiration);

                    foreach (var expiration in expirations)
                    {
                        var strikes = expiration.strikes.ToList();

                        sellStrike = strikes.Where(x => Convert.ToDecimal(x.strikeprice) == desiredStrike).FirstOrDefault();
                        buyStrike = strikes.Where(x => Convert.ToDecimal(x.strikeprice) == desiredStrike - desiredRisk).FirstOrDefault();

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
                    Console.WriteLine("Spread is invalid or not found.");
                    return;
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

                var preview = await tastyBot.doDryRun(primaryAccount.account.accountnumber, creditSpread);

                if (preview.warnings.Length == 0)
                {
                    if (preview.order.status == "Received")
                    {
                        var newBuyingPower = Convert.ToDecimal(preview.buyingpowereffect.newbuyingpower);

                        if (newBuyingPower > maintainAtLeastThisMuchBuyingPower)
                        {
                            // Warning: Actually places an order when _liveOrdersEnabled is true (default is false).
                            var order = await tastyBot.placeOrder(primaryAccount.account.accountnumber, creditSpread);

                            if (order.order.status == "Routed") {
                                Console.WriteLine("Order placed.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                await tastyBot.Terminate();
                quoteMachine.Terminate();
            }
        }
    }
}