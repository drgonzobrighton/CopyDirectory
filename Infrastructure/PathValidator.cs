using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CopyDirectory.Validation
{
    public class PathValidator : IPathValidator
    {

        public List<ValidationMessage> ValidateSourcePath(string sourcePath)
        {
            List<ValidationMessage> sourceValidationMessages = new();

            ValidateCommonPathAttributes(sourcePath, sourceValidationMessages);

            var sourceDirInfo = new DirectoryInfo(sourcePath);

            if (!sourceDirInfo.Exists)
            {
                sourceValidationMessages.Add(new("The source directory does not exist", ValidationMessageType.Error));
            }

            return sourceValidationMessages;
        }


        public List<ValidationMessage> ValidateTargetPath(string targetPath, string sourcePath)
        {
            List<ValidationMessage> targetValidationMessageges = new();

            ValidateCommonPathAttributes(targetPath, targetValidationMessageges);

            if (Path.IsPathRooted(targetPath))
            {
                if (!DriveInfo.GetDrives().Any(x => x.Name.ToLower() == Path.GetPathRoot(targetPath).ToLower()))
                {
                    targetValidationMessageges.Add(new("The target directory root does not exist", ValidationMessageType.Error));
                }
            }
            else
            {
                targetValidationMessageges.Add(new("The target directory path must have a root", ValidationMessageType.Error));
            }

            var sourceDirInfo = new DirectoryInfo(sourcePath);
            var targetDirInfo = new DirectoryInfo(targetPath);

            if (sourceDirInfo.FullName.ToLower() == targetDirInfo.FullName.ToLower())
            {
                targetValidationMessageges.Add(new("The target directory is the same as the source directory", ValidationMessageType.Error));
            }

            return targetValidationMessageges;
        }



        private void ValidateCommonPathAttributes(string path, List<ValidationMessage> targetValidationMessageges)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                targetValidationMessageges.Add(new("Path cannot be empty", ValidationMessageType.Error));
            }
        }
    }
}
