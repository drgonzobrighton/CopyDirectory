using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Services
{
    public class CopyDirectoryService : ICopyDirectoryService
    {
        private DirectoryInfo _sourceDirectoryInfo;
        private DirectoryInfo _targetDirectoryInfo;

        public void Init(string sourceDirectoryPath, string targetDirectoryPath)
        {
            _sourceDirectoryInfo = new(sourceDirectoryPath);
            _targetDirectoryInfo = new(targetDirectoryPath);
        }

        public async Task CopyDirectory(IProgressLogger progressLogger)
        {
            progressLogger.LogProgress("Starting..");

            await CopyDirectoryInternal(_sourceDirectoryInfo, _targetDirectoryInfo, progressLogger);

            progressLogger.LogProgress("Finished");
        }

        private async Task CopyDirectoryInternal(DirectoryInfo sourceDirInfo, DirectoryInfo targetDirInfo, IProgressLogger progressLogger)
        {
            Directory.CreateDirectory(targetDirInfo.FullName);

            foreach (FileInfo fi in sourceDirInfo.GetFiles())
            {
                progressLogger.LogProgress($"Copying {sourceDirInfo.FullName}/{fi.Name} to {targetDirInfo.FullName}/{fi.Name}");
                fi.CopyTo(Path.Combine(targetDirInfo.FullName, fi.Name), true);
            }

            foreach (DirectoryInfo diSourceSubDir in sourceDirInfo.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = targetDirInfo.CreateSubdirectory(diSourceSubDir.Name);
                progressLogger.LogProgress($"Created {nextTargetSubDir.FullName}");
                await CopyDirectoryInternal(diSourceSubDir, nextTargetSubDir, progressLogger);
            }

        }

        public bool ValidatePaths(out List<ValidationMessage> validationMessages)
        {
            validationMessages = new();

            if (_sourceDirectoryInfo.Exists)
            {
                if (_sourceDirectoryInfo.FullName.ToLower() == _targetDirectoryInfo.FullName.ToLower())
                {
                    validationMessages.Add(new("The target directory is the same as the source directory", ValidationMessageType.Warning));
                }
            }
            else
            {
                validationMessages.Add(new("The source directory does not exist", ValidationMessageType.Error));

            }

            if (!IsValidPath(_targetDirectoryInfo.FullName))
            {
                validationMessages.Add(new("The target directory path is invalid", ValidationMessageType.Error));
            }

            return validationMessages.Count == 0;
        }

        private static bool IsValidPath(string path, bool exactPath = true)
        {
            bool isValid = true;

            try
            {

                if (exactPath)
                {
                    string root = Path.GetPathRoot(path);
                    isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
                }
                else
                {
                    isValid = Path.IsPathRooted(path);
                }
            }
            catch (Exception ex)
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
