using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface ICopyDirectoryService
    {
        void Init(string sourceDirectoryPath, string targetDirectoryPath);
        Task CopyDirectory(IProgressLogger progressLogger);

        bool ValidatePaths(out List<ValidationMessage> validationMessages);
    }
}
