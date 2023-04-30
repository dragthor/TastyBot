using System;
using System.Threading.Tasks;
using System.Linq;
using TastyBot.Strategy;
using TastyBot.Library;

namespace TastyBot
{
    public partial class Program
    {
        private const string BaseUrl = "https://api.tastyworks.com";
        private const string BaseQuoteUrl = "https://api.stockdata.org";

        private const int TimeOut = 10;
        
        public static async Task Main(string[] args)
        {
            ILogger logger = new ConsoleLogger();

            var tastyBot = Library.TastyBot.CreateDebugInstance(logger, SecretName, SecretSauce, BaseUrl, TimeOut);
            var quoteMachine = Library.ThirdParty.StockDataOrg.CreateInstance(logger, BaseQuoteUrl, TimeOut, StockDataOrgApiToken);
            
            try
            {
                var sessionInfo = await tastyBot.getAuthorization();

                logger.Info($"Welcome: {sessionInfo.user.username}");

                var accounts = await tastyBot.getAccounts();

                accounts.items.ToList().ForEach(account =>
                {
                    logger.Info($"Account: {account.account.accountnumber}");
                });

                var primaryAccount = accounts.items.First();

                ITastyBotStrategy p = PutSpread.CreateInstance(tastyBot, quoteMachine, primaryAccount, "SPY", 5, 50);
         
                var result = await p.MakeAttempt();

                switch (result) {
                    case StrategyAttemptResult.OrderEntered:
                        logger.Info("Order entered.");
                        break;
                    case StrategyAttemptResult.StrikeNotFound:
                        logger.Info("Unable to find desired strike(s).");
                        break;
                    case StrategyAttemptResult.InvalidSetup:
                        logger.Info("Invalid strategy or order setup.");
                        break;
                    case StrategyAttemptResult.OrderRoutingError:
                        logger.Info("Order routing issue.");
                        break;
                    case StrategyAttemptResult.OrderWarnings:
                        logger.Info("Order has warnings.");
                        break;
                    case StrategyAttemptResult.OrderNotReceived:
                        logger.Info("Order was not received.");
                        break;
                    case StrategyAttemptResult.NothingToDo:
                    default:
                        logger.Info("Nothing to do at this time.");
                        break;
                }  
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            finally
            {
                await tastyBot.Terminate();
                await quoteMachine.Terminate();
            }
        }

        public class ConsoleLogger : ILogger
        {
            public void Info(string message)
            {
                if (string.IsNullOrWhiteSpace(message)) return;

                Console.WriteLine(message);
            }

            public void Error(string message)
            {
                if (string.IsNullOrWhiteSpace(message)) return;

                Console.WriteLine(message);
            }
        }
    }
}