namespace TastyBot.Library
{
    public interface ILogger
    {
        void Info(string message);
        void Error(string message);
    }
}