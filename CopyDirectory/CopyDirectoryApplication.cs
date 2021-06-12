using Services;
using System;
using System.Threading.Tasks;

namespace CopyDirectory
{
    public class CopyDirectoryApplication
    {
        private readonly ICopyDirectoryService _directoryCopyService;
        private readonly IMessageLogger _progressLogger;

        public CopyDirectoryApplication(ICopyDirectoryService directoryCopyService, IMessageLogger progressLogger)
        {
            _directoryCopyService = directoryCopyService;
            _progressLogger = progressLogger;
        }

        public async Task Run()
        {
            var exitApp = false;

            do
            {

                if (HandleUserInput())
                {
                    await CopyDirectory();
                }

                PromtExit(ref exitApp);

            } while (!exitApp);
        }

        private async Task CopyDirectory()
        {
            try
            {
                await _directoryCopyService.CopyDirectory();
            }
            catch (Exception e)
            {

                _progressLogger.LogMessage(e.Message, MessageType.Error);

            }

        }

        private bool HandleUserInput()
        {
            var userInputIsValid = true;

            (string sourceDir, string targetDir) = GetTargetAndSourceDirectories();

            _directoryCopyService.Init(sourceDir, targetDir, _progressLogger);

            userInputIsValid = ValidateUserInput(userInputIsValid);

            return userInputIsValid;
        }

        private bool ValidateUserInput(bool userInputIsValid)
        {
            if (!_directoryCopyService.ValidatePaths(out var validationMessages))
            {
                userInputIsValid = false;

                foreach (var message in validationMessages)
                {
                    _progressLogger.LogMessage(message.Message, message.MessageType);

                }

            }

            return userInputIsValid;
        }

        private void PromtExit(ref bool exitApp)
        {
            Console.WriteLine("Would you like to continue? Press Y for yes or any other key to exit the application.");
            var response = Console.ReadLine();

            if (response.ToLower() != "y")
            {
                exitApp = true;
            }
        }



        private (string sourceDir, string targetDir) GetTargetAndSourceDirectories()
        {
            var sourceDir = GetDirectoryPathFromUserInput("source");
            var targetDir = GetDirectoryPathFromUserInput("target");

            return (sourceDir, targetDir);
        }

        private string GetDirectoryPathFromUserInput(string pathType)
        {
            string dirPath;
            while (true)
            {
                Console.WriteLine($"Please enter the {pathType} directory path");
                dirPath = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(dirPath))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Path cannot be empty");
                    Console.ResetColor();
                }
                else
                {
                    break;
                }
            }

            return dirPath;
        }
    }
}
