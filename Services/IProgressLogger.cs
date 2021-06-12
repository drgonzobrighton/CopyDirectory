namespace Services
{
    public interface IProgressLogger
    {
        void LogProgress(string progress, MessageType messageType = MessageType.Info);
    }
}
