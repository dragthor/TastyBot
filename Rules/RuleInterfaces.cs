namespace TastyBot.Rules
{
    public interface IRuleQuote {
        float price { get; }
        float dayChange { get; }
    }
}