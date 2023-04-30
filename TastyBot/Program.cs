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

                // This is not an iron condor.  Based on the current strategy, only one will be filled.
                ITastyBotStrategy p = PutSpread.CreateInstance(tastyBot, quoteMachine, primaryAccount, "SPY", 5, 50);

                var putSpreadResult = await p.MakeAttempt();

                ConsoleLogger.ProcessResult(logger, putSpreadResult);

                ITastyBotStrategy c = CallSpread.CreateInstance(tastyBot, quoteMachine, primaryAccount, "SPY", 5, 50);

                var callSpreadResult = await c.MakeAttempt();

                ConsoleLogger.ProcessResult(logger, callSpreadResult);
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
    }
}