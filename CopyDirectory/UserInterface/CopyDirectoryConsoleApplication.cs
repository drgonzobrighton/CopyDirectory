using CopyDirectory.Logging;
using CopyDirectory.Presentation.Utilities;
using CopyDirectory.Services;
using CopyDirectory.Validation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CopyDirectory.UserInterface
{
    public class CopyDirectoryConsoleApplication
    {
        private readonly IFileService _directoryCopyService;
        private readonly IMessageLogger _messageLogger;
        private readonly IPathValidator _pathValidator;

        //  private List<ValidationMessage> _userInputValidationMessages;
        private bool _canCopyFiles = true;

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

                // _directoryCopyService.OnPathsValidated(OnUserInputValidated);

                if (_canCopyFiles)
                {
                    await CopyDirectory(sourceDirPath, targetDirPath);
                }


                PromtExit(ref exitApp);

            } while (!exitApp);
        }

        //public void OnUserInputValidated(string message, MessageType messageType)
        //{
        //    _messageLogger.LogMessage(message, messageType);
        //    _messageLogger.LogMessage("\n Please press [Y] to copy the source directory or any other key to continue.");

        //    string response;

        //    while (true)
        //    {

        //        response = Console.ReadLine();

        //        if (string.IsNullOrWhiteSpace(response) || (response.ToLower() != "y" && response.ToLower() != "n"))
        //        {
        //            _messageLogger.LogMessage("Please either press [Y] or [N]", MessageType.Error);
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }

        //    if (response.ToLower() == "n")
        //    {
        //        _canCopyFiles = false;
        //    }
        //}

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

        public void StatusCallback(string message, FileCopyStatus fileCopyStatus)
        {
            _messageLogger.LogMessage(message, Converters.ConvertFileStatusToLogType(fileCopyStatus));
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
                Console.WriteLine($"\nPlease enter the {pathType} directory path:");
                dirPath = Console.ReadLine();

                var validationMessages = pathType == PathType.Source ? _pathValidator.ValidateSourcePath(dirPath) : _pathValidator.ValidateTargetPath(dirPath, sourcePath);

                if (validationMessages.Count() > 0)
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

                //check warning
            }

            return dirPath;
        }
    }
}
