using Services;
using System;
using System.Threading.Tasks;

namespace CopyDirectory
{
    public class CopyDirectoryApplication
    {
        private readonly ICopyDirectoryService _directoryCopyService;

        public CopyDirectoryApplication(ICopyDirectoryService directoryCopyService)
        {
            _directoryCopyService = directoryCopyService;
        }

        public async Task Run()
        {
            (string sourceDir, string targetDir) = GetTargetAndSourceDirectories();

            if (!_directoryCopyService.ValidatePaths(sourceDir, targetDir, out var validationMessages))
            {
                foreach (var message in validationMessages)
                {
                    var colour = GetForeGroundColour(message.MessageType);
                    Console.ForegroundColor = colour;
                    Console.WriteLine(message.Message);
                    Console.ResetColor();
                }

                return;
            }

            await _directoryCopyService.CopyDirectory(sourceDir, targetDir);
        }

        private static ConsoleColor GetForeGroundColour(ValidationMessageType messageType)
        {
            return messageType switch
            {
                ValidationMessageType.Warning => ConsoleColor.Yellow,
                ValidationMessageType.Danger => ConsoleColor.Red,
                _ => ConsoleColor.Yellow
            };
        }

        private (string sourceDir, string targetDir) GetTargetAndSourceDirectories()
        {
            throw new NotImplementedException();
        }
    }
}
