using System;
using System.Threading.Tasks;

namespace CopyDirectory.Services
{
    public interface IFileService
    {
        Task CopyDirectory(string sourceDirPath, string targetDirPath, Action<string, FileCopyStatus> statusCallback);
    }
}
