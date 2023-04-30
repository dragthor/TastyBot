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

                if (result == 1)
                {
                    Console.WriteLine($"Order entered.");
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