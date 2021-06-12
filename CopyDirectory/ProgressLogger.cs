using Services;
using System;

namespace CopyDirectory
{
    public class ProgressLogger : IProgressLogger
    {
        public void LogProgress(string progress)
        {
            Console.WriteLine(progress);
        }
    }
}
