
namespace CopyDirectory.Logging
{
    public interface IMessageLogger
    {
        void LogMessage(string message, LogType messageType = LogType.Info);
    }
}
