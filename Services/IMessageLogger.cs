namespace Services
{
    public interface IMessageLogger
    {
        void LogMessage(string message, MessageType messageType = MessageType.Info);
    }
}
