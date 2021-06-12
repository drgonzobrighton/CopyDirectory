using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface ICopyDirectoryService
    {
        void Init(string sourceDirectoryPath, string targetDirectoryPath, IMessageLogger messageLogger);
        Task CopyDirectory();
        bool ValidatePaths(out List<ValidationMessage> validationMessages);
    }
}
