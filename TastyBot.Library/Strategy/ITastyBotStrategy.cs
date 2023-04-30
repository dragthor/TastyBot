namespace TastyBot.Strategy
{
    public interface ITastyBotStrategy
    {
        Task<StrategyAttemptResult> MakeAttempt();
    }
}
