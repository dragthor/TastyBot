using System;
using System.Threading.Tasks;
using System.Linq;
using TastyBot.Strategy;

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
            var quoteMachine = Library.ThirdParty.StockDataOrg.CreateInstance(BaseQuoteUrl, TimeOut, StockDataOrgApiToken);

            try
            {
                var sessionInfo = await tastyBot.getAuthorization();

                Console.WriteLine($"Welcome: {sessionInfo.user.username}");

                var accounts = await tastyBot.getAccounts();

                accounts.items.ToList().ForEach(account =>
                {
                    Console.WriteLine($"Account: {account.account.accountnumber}");
                });

                var primaryAccount = accounts.items.First();

                ITastyBotStrategy p = PutSpread.CreateInstance(tastyBot, quoteMachine, primaryAccount, "SPY", 5, 50);
         
                var result = await p.MakeAttempt();

                switch (result) {
                    case StrategyAttemptResult.OrderEntered:
                        Console.WriteLine("Order entered.");
                        break;
                    case StrategyAttemptResult.StrikeNotFound:
                        Console.WriteLine("Unable to find desired strike(s).");
                        break;
                    case StrategyAttemptResult.InvalidSetup:
                        Console.WriteLine("Invalid strategy or order setup.");
                        break;
                    case StrategyAttemptResult.OrderRoutingError:
                        Console.WriteLine("Order routing issue.");
                        break;
                    case StrategyAttemptResult.OrderWarnings:
                        Console.WriteLine("Order has warnings.");
                        break;
                    case StrategyAttemptResult.OrderNotReceived:
                        Console.WriteLine("Order was not received.");
                        break;
                    case StrategyAttemptResult.NothingToDo:
                    default:
                        Console.WriteLine("Nothing to do at this time.");
                        break;
                }  
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                await tastyBot.Terminate();
                await quoteMachine.Terminate();
            }
        }
    }
}