using Services;
using System;

namespace CopyDirectory.Utilities
{
    public static class Converters
    {
        public static ConsoleColor ConvertMessageTypeToConsoleColour(MessageType messageType)
        {
            return messageType switch
            {
                MessageType.Warning => ConsoleColor.Yellow,
                MessageType.Error => ConsoleColor.Red,
                MessageType.Success => ConsoleColor.Green,
                MessageType.Info or _ => ConsoleColor.White,
            };
        }
    }
}
