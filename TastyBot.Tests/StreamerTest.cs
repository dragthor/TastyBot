using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace TastyBot.Tests
{
    [TestClass]
    public class StreamerTest : BaseTest
    {
        [TestMethod]
        public async Task able_to_get_quote_streamer_info()
        {
            var tastyBot = Library.TastyBot.CreateDebugInstance(User, Password, BaseUrl, 10);

            try
            {
                var sessionInfo = await tastyBot.getAuthorization();

                var streamerInfo = await tastyBot.getStreamerTokens();

                Assert.IsFalse(string.IsNullOrWhiteSpace(streamerInfo.token));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                await tastyBot.Terminate();
            }
        }
    }
}
