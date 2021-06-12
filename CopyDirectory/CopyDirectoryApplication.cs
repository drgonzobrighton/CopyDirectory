using Services;
using System;
using System.Threading.Tasks;

namespace CopyDirectory
{
    public class CopyDirectoryApplication
    {
        private readonly ICopyDirectoryService _directoryCopyService;
        private readonly IProgressLogger _progressLogger;

        public CopyDirectoryApplication(ICopyDirectoryService directoryCopyService, IProgressLogger progressLogger)
        {
            _directoryCopyService = directoryCopyService;
            _progressLogger = progressLogger;
        }

        public async Task Run()
        {
            (string sourceDir, string targetDir) = GetTargetAndSourceDirectories();

            _directoryCopyService.Init(sourceDir, targetDir);

            if (!_directoryCopyService.ValidatePaths(out var validationMessages))
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

            try
            {
                await _directoryCopyService.CopyDirectory(_progressLogger);
            }
            catch (Exception e)
            {

                _progressLogger.LogProgress(e.Message);
            }
        }

        private static ConsoleColor GetForeGroundColour(ValidationMessageType messageType)
        {
            return messageType switch
            {
                ValidationMessageType.Warning => ConsoleColor.Yellow,
                ValidationMessageType.Error => ConsoleColor.Red,
                _ => ConsoleColor.Yellow
            };
        }

        private (string sourceDir, string targetDir) GetTargetAndSourceDirectories()
        {
            string sourceDir = null;
            string targetDir = null;

            while (true)
            {
                Console.WriteLine("Please enter the source directory");
                sourceDir = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(sourceDir))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a valid source directory");
                    Console.ResetColor();
                }
                else
                {
                    break;
                }
            }

            while (true)
            {
                Console.WriteLine("Please enter the target directory");
                targetDir = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(targetDir))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a valid target directory");
                    Console.ResetColor();
                }
                else
                {
                    break;
                }
            }

            return (sourceDir, targetDir);
        }
    }
}
