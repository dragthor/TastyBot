using TastyBot.Rules;

namespace TastyBot.Library
{
    public interface IQuoteMachine
    {
        Task<IRuleQuote> getQuote(string ticker);
        Task Terminate();
    }
}