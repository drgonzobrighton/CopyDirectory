using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface ICopyDirectoryService
    {
        Task CopyDirectory(string sourceDirectory, string targetDirectory);

        bool ValidatePaths(string sourceDirectory, string targetDirectory, out IEnumerable<IValidationMessage> validationMessages);
    }
}
