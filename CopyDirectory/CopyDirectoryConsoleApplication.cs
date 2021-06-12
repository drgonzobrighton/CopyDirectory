﻿using Services;
using System;
using System.Threading.Tasks;

namespace CopyDirectory
{
    public class CopyDirectoryConsoleApplication : ICopyDirectoryApplication
    {
        private readonly ICopyDirectoryService _directoryCopyService;
        private readonly IMessageLogger _messageLogger;

        //  private List<ValidationMessage> _userInputValidationMessages;
        private bool _canCopyFiles = true;

        public CopyDirectoryConsoleApplication(ICopyDirectoryService directoryCopyService, IMessageLogger progressLogger)
        {
            _directoryCopyService = directoryCopyService;
            _messageLogger = progressLogger;
        }

        public async Task Run()
        {
            var exitApp = false;

            do
            {

                if (ValidateUserInput())
                {
                    _directoryCopyService.OnPathsValidated(OnUserInputValidated);

                    if (_canCopyFiles)
                    {
                        await CopyDirectory();
                    }
                }

                PromtExit(ref exitApp);

            } while (!exitApp);
        }

        public void OnUserInputValidated(string message, MessageType messageType)
        {
            _messageLogger.LogMessage(message, messageType);
            _messageLogger.LogMessage("\n Please press Y to copy the source directory or any other key to continue.");

            string response;

            while (true)
            {

                response = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(response) || (response.ToLower() != "y" && response.ToLower() != "n"))
                {
                    _messageLogger.LogMessage("Please either press Y or N", MessageType.Error);
                }
                else
                {
                    break;
                }
            }

            if (response.ToLower() == "n")
            {
                _canCopyFiles = false;
            }
        }

        private async Task CopyDirectory()
        {
            try
            {
                await _directoryCopyService.CopyDirectory();
            }
            catch (Exception e)
            {

                _messageLogger.LogMessage(e.Message, MessageType.Error);

            }

        }

        private bool ValidateUserInput()
        {

            (string sourceDir, string targetDir) = GetTargetAndSourceDirectoriesFromUser();

            _directoryCopyService.Init(sourceDir, targetDir, _messageLogger);

            if (!_directoryCopyService.ValidatePaths())
            {

                return false;
            }

            return true;
        }



        private void PromtExit(ref bool exitApp)
        {
            Console.WriteLine("\nWould you like to continue? Press Y for yes or any other key to exit the application.");
            var response = Console.ReadLine();

            if (response.ToLower() != "y")
            {
                exitApp = true;
            }
        }



        private (string sourceDir, string targetDir) GetTargetAndSourceDirectoriesFromUser()
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
                Console.WriteLine($"\nPlease enter the {pathType} directory path:");
                dirPath = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(dirPath))
                {
                    //Console.ForegroundColor = ConsoleColor.Red;
                    //Console.WriteLine("Path cannot be empty\n");
                    //Console.ResetColor();
                    _messageLogger.LogMessage("Path cannot be empty\n", MessageType.Error);
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