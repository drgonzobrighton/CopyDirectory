using CopyDirectory.Logging;
using CopyDirectory.Presentation.Utilities;
using System;

namespace CopyDirectory.UserInterface
{
    public class ConsoleMessageLogger : IMessageLogger
    {
        public void LogMessage(string message, LogType messageType = LogType.Info)
        {
            Console.ForegroundColor = Converters.ConvertLogTypeToConsoleColour(messageType);
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
