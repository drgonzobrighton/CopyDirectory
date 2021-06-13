using System;
using System.IO;
using System.Threading.Tasks;

namespace CopyDirectory.Services
{
    public class FileService : IFileService
    {

        public async Task CopyDirectory(string sourceDirPath, string targetDirPath, Action<string, FileCopyStatus> statusCallback)
        {
            statusCallback("Starting..", FileCopyStatus.Started);

            await CopyDirectoryInternal(new DirectoryInfo(sourceDirPath), new DirectoryInfo(targetDirPath), statusCallback);

            statusCallback("Finished", FileCopyStatus.Finished);
        }



        private async Task CopyDirectoryInternal(DirectoryInfo sourceDirInfo, DirectoryInfo targetDirInfo, Action<string, FileCopyStatus> statusCallback)
        {
            try
            {
                Directory.CreateDirectory(targetDirInfo.FullName);

                foreach (var fi in sourceDirInfo.GetFiles())
                {
                    statusCallback($"Copying {sourceDirInfo.FullName}/{fi.Name} to {targetDirInfo.FullName}/{fi.Name}", FileCopyStatus.Copied);
                    fi.CopyTo(Path.Combine(targetDirInfo.FullName, fi.Name), true);
                }

                foreach (var diSourceSubDir in sourceDirInfo.GetDirectories())
                {
                    DirectoryInfo nextTargetSubDir = targetDirInfo.CreateSubdirectory(diSourceSubDir.Name);
                    statusCallback($"Created {nextTargetSubDir.FullName}", FileCopyStatus.Copied);
                    await CopyDirectoryInternal(diSourceSubDir, nextTargetSubDir, statusCallback);
                }
            }
            catch (Exception e)
            {

                statusCallback(e.Message, FileCopyStatus.Errored);
            }

        }

    }
}
