using CopyDirectory.Utilities;
using Services;
using System;

namespace CopyDirectory
{
    public class ProgressLogger : IProgressLogger
    {
        public void LogProgress(string progress, MessageType messageType = MessageType.Info)
        {
            Console.ForegroundColor = Converters.ConvertMessageTypeToConsoleColour(messageType);
            Console.WriteLine(progress);
            Console.ResetColor();
        }
    }
}
