using System;
using TastyBot.Library;
using TastyBot.Strategy;

namespace TastyBot
{
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

        public static void ProcessResult(ILogger logger, StrategyAttemptResult result)
        {
            switch (result)
            {
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
    }
}
