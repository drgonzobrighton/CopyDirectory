using System.Collections.Generic;

namespace CopyDirectory.Validation
{
    public interface IPathValidator
    {
        List<ValidationMessage> ValidateSourcePath(string sourcePath);
        List<ValidationMessage> ValidateTargetPath(string targetPath, string sourcePath);
    }
}
