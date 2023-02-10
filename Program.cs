using System;
using System.Threading.Tasks;
using System.Linq;
using TastyBot.Models;

namespace TastyBot
{
    public partial class Program
    {
        private const string BaseUrl = "https://api.tastyworks.com";
        private const int TimeOut = 10;
       
        public static async Task Main(string[] args)
        {
            var tastyBot = Library.TastyBot.CreateInstance(SecretName, SecretSauce, BaseUrl, TimeOut);

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

                Console.WriteLine("Cash: " + balance.cashbalance + ", Maintenance: " + balance.maintenanceexcess + ", Reg-T Margin: " + balance.regtmarginrequirement + ", Futures Margin; " + balance.futuresmarginrequirement);

                var result = await tastyBot.processRules();

                if (result.Any() && result.All(x => x.answer == true))
                {
                    // TODO: Implement
                    // Sell a $25 wide SPX put spread at the 10 delta
                } else
                {
                    Console.WriteLine("Nothing to do at this time.");
                }

                var shortLeg = new Leg()
                {
                    instrumenttype = "Equity Option",
                    symbol = "SPX   230317P03700000",
                    action = "Sell to Open",
                    quantity = "1"
                };

                var longLeg = new Leg()
                {
                    instrumenttype = "Equity Option",
                    symbol = "SPX   230317P03695000",
                    action = "Buy to Open",
                    quantity = "1"
                };

                var creditSpread = new TastySpread() {
                    source = "WBT-ember;",
                    ordertype = "Limit",
                    timeinforce = "GTC",
                    price = ".95",
                    priceeffect = "Credit",
                    legs = new Leg[] { shortLeg, longLeg }  
                };

                var preview = await tastyBot.doDryRun(primaryAccount.account.accountnumber, creditSpread);

                // Warning: Actually places an order when _liveOrdersEnabled is true (default is false).
                // var order = await tastyBot.placeOrder(primaryAccount.account.accountnumber, creditSpread);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                tastyBot.Terminate();
            }
        }
    }
}