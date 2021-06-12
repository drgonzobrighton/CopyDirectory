using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class CopyDirectoryService : ICopyDirectoryService
    {
        //private DirectoryInfo _sourceDirectoryInfo;
        //private DirectoryInfo _targetDirectoryInfo;
        private string _sourceDirectoryPath;
        private string _targetDirectoryPath;
        IMessageLogger _messageLogger;

        public void Init(string sourceDirectoryPath, string targetDirectoryPath, IMessageLogger messageLogger)
        {
            _sourceDirectoryPath = sourceDirectoryPath;
            _targetDirectoryPath = targetDirectoryPath;
            _messageLogger = messageLogger;
        }

        public async Task CopyDirectory()
        {
            _messageLogger.LogMessage("Starting..");

            await CopyDirectoryInternal(new DirectoryInfo(_sourceDirectoryPath), new DirectoryInfo(_targetDirectoryPath));

            _messageLogger.LogMessage("Finished", MessageType.Success);
        }



        public bool ValidatePaths()
        {
            var isValid = true;

            try
            {
                var sourceDirInfo = new DirectoryInfo(_sourceDirectoryPath);


                if (Path.IsPathRooted(_targetDirectoryPath))
                {
                    if (!DriveInfo.GetDrives().Any(x => x.Name.ToLower() == Path.GetPathRoot(_targetDirectoryPath)))
                    {
                        _messageLogger.LogMessage("The target directory root does not exist", MessageType.Error);
                        isValid = false;
                    }
                }
                else
                {
                    _messageLogger.LogMessage("The target directory path must have a root", MessageType.Error);
                    isValid = false;

                }

                if (sourceDirInfo.Exists)
                {
                    var targetDirInfo = new DirectoryInfo(_targetDirectoryPath);

                    if (sourceDirInfo.FullName.ToLower() == targetDirInfo.FullName.ToLower())
                    {
                        _messageLogger.LogMessage("The target directory is the same as the source directory", MessageType.Warning);
                        isValid = false;

                    }
                }
                else
                {
                    _messageLogger.LogMessage("The source directory does not exist", MessageType.Error);
                    isValid = false;


                }
            }
            catch (Exception e)
            {

                _messageLogger.LogMessage(e.Message, MessageType.Error);
                isValid = false;

            }

            return isValid;
        }

        public void OnPathsValidated(Action<string, MessageType> onvalidatedDelegate)
        {
            if (new DirectoryInfo(_targetDirectoryPath).Exists)
            {
                onvalidatedDelegate("The source path already exists. Copying will override the folder's contents.", MessageType.Warning);
            }
        }

        private async Task CopyDirectoryInternal(DirectoryInfo sourceDirInfo, DirectoryInfo targetDirInfo)
        {
            Directory.CreateDirectory(targetDirInfo.FullName);

            foreach (var fi in sourceDirInfo.GetFiles())
            {
                _messageLogger.LogMessage($"Copying {sourceDirInfo.FullName}/{fi.Name} to {targetDirInfo.FullName}/{fi.Name}");
                fi.CopyTo(Path.Combine(targetDirInfo.FullName, fi.Name), true);
            }

            foreach (var diSourceSubDir in sourceDirInfo.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = targetDirInfo.CreateSubdirectory(diSourceSubDir.Name);
                _messageLogger.LogMessage($"Created {nextTargetSubDir.FullName}");
                await CopyDirectoryInternal(diSourceSubDir, nextTargetSubDir);
            }

        }


    }
}
