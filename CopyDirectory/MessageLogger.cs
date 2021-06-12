using CopyDirectory.Utilities;
using Services;
using System;

namespace CopyDirectory
{
    public class MessageLogger : IMessageLogger
    {
        public void LogMessage(string message, MessageType messageType = MessageType.Info)
        {
            Console.ForegroundColor = Converters.ConvertMessageTypeToConsoleColour(messageType);
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
