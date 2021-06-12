using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class CopyDirectoryService : ICopyDirectoryService
    {


        public bool ValidatePaths(string sourceDirectory, string targetDirectory, out IEnumerable<IValidationMessage> validationMessages)
        {
            throw new NotImplementedException();
        }

        async Task ICopyDirectoryService.CopyDirectory(string sourceDirectory, string targetDirectory)
        {
            throw new NotImplementedException();
        }
    }
}
