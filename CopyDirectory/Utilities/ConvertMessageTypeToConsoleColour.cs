using CopyDirectory.Logging;
using CopyDirectory.Services;
using CopyDirectory.Validation;
using System;

namespace CopyDirectory.Presentation.Utilities
{
    public static class Converters
    {
        public static ConsoleColor ConvertLogTypeToConsoleColour(LogType messageType)
        {
            return messageType switch
            {
                LogType.Warning => ConsoleColor.Yellow,
                LogType.Error => ConsoleColor.Red,
                LogType.Success => ConsoleColor.Green,
                LogType.Info or _ => ConsoleColor.White,
            };
        }

        public static LogType ConvertValidationMessageTypeToLogType(ValidationMessageType validationMessageType)
        {
            return validationMessageType switch
            {
                ValidationMessageType.Error => LogType.Error,
                ValidationMessageType.Warning => LogType.Warning,
                _ => LogType.Info
            };
        }

        public static LogType ConvertFileStatusToLogType(FileCopyStatus fileCopyStatus)
        {
            return fileCopyStatus switch
            {
                FileCopyStatus.Errored => LogType.Error,
                FileCopyStatus.Finished => LogType.Success,
                FileCopyStatus.Copied or FileCopyStatus.Started or _ => LogType.Info
            };
        }
    }
}
