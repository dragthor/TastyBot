using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace TastyBot.Tests
{
    [TestClass]
    public class AuthorizationTest : BaseTest
    {
        [TestMethod]
        public async Task able_to_authorize()
        {
            var tastyBot = Library.TastyBot.CreateDebugInstance(User, Password, BaseUrl, 10);

            try
            {
                var sessionInfo = await tastyBot.getAuthorization();

                var authToken = sessionInfo.sessiontoken;
                var user = sessionInfo.user.username;
                var email = sessionInfo.user.email;

                Assert.IsFalse(string.IsNullOrWhiteSpace(authToken));
                Assert.AreEqual(User, user);

            } catch (Exception ex) {
                throw;
            }
            finally
            {
                await tastyBot.Terminate();
            }
        }
    }
}