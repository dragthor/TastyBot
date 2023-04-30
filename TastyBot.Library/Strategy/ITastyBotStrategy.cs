namespace TastyBot.Strategy
{
    public interface ITastyBotStrategy
    {
        Task<int> MakeAttempt();
    }
}
