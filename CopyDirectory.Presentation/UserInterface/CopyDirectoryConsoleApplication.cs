using CopyDirectory.Logging;
using CopyDirectory.Presentation.Utilities;
using CopyDirectory.Services;
using CopyDirectory.Validation;
using System;
using System.Threading.Tasks;

namespace CopyDirectory.Presentation.UserInterface
{
    public class CopyDirectoryConsoleApplication
    {
        private readonly IFileService _directoryCopyService;
        private readonly IMessageLogger _messageLogger;
        private readonly IPathValidator _pathValidator;

        public CopyDirectoryConsoleApplication(IFileService directoryCopyService, IMessageLogger progressLogger, IPathValidator pathValidator)
        {
            _directoryCopyService = directoryCopyService;
            _messageLogger = progressLogger;
            _pathValidator = pathValidator;
        }

        public async Task Run()
        {
            var exitApp = false;

            do
            {
                (string sourceDirPath, string targetDirPath) = GetSourceAndTargetPathsFromUser();

                await CopyDirectory(sourceDirPath, targetDirPath);

                PromtExit(ref exitApp);

            } while (!exitApp);
        }

        public void StatusCallback(string message, FileCopyStatus fileCopyStatus)
        {
            _messageLogger.LogMessage(message, Converters.ConvertFileStatusToLogType(fileCopyStatus));
        }

        private async Task CopyDirectory(string sourceDirPath, string targetDirPath)
        {
            try
            {
                await _directoryCopyService.CopyDirectory(sourceDirPath, targetDirPath, StatusCallback);
            }
            catch (Exception e)
            {
                _messageLogger.LogMessage(e.Message, LogType.Error);
            }

        }


        private void PromtExit(ref bool exitApp)
        {
            Console.WriteLine("\nWould you like to copy another directory? Press [Y] for yes or any other key to exit the application.");
            var response = Console.ReadLine();

            if (response.ToLower() != "y")
            {
                exitApp = true;
            }
        }


        private (string sourceDir, string targetDir) GetSourceAndTargetPathsFromUser()
        {
            var sourceDir = GetValidDirectoryPathFromUserInput(PathType.Source);
            var targetDir = GetValidDirectoryPathFromUserInput(PathType.Target, sourceDir);

            return (sourceDir, targetDir);
        }

        private string GetValidDirectoryPathFromUserInput(PathType pathType, string sourcePath = null)
        {
            string dirPath;

            while (true)
            {
                Console.WriteLine($"\nPlease enter the {pathType.ToString().ToLower()} directory path:");
                dirPath = Console.ReadLine();

                var validationMessages = pathType == PathType.Source ? _pathValidator.ValidateSourcePath(dirPath) : _pathValidator.ValidateTargetPath(dirPath, sourcePath);

                if (validationMessages.Count > 0)
                {
                    foreach (var message in validationMessages)
                    {
                        _messageLogger.LogMessage(message.Message, Converters.ConvertValidationMessageTypeToLogType(message.ValidationMessageType));
                    }

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
