﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
            var logger = new Mock<Library.ILogger>();
            var tastyBot = Library.TastyBot.CreateDebugInstance(logger.Object, User, Password, BaseUrl, 10);

            try
            {
                var sessionInfo = await tastyBot.getAuthorization();

                var streamerInfo = await tastyBot.getStreamerTokens();

                Assert.IsFalse(string.IsNullOrWhiteSpace(streamerInfo.token));
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await tastyBot.Terminate();
            }
        }
    }
}
