using CopyDirectory.Logging;
using CopyDirectory.Services;
using CopyDirectory.UserInterface;
using CopyDirectory.Validation;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace CopyDirectory
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            await serviceProvider.GetService<CopyDirectoryConsoleApplication>().Run();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<CopyDirectoryConsoleApplication>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IMessageLogger, ConsoleMessageLogger>();
            services.AddSingleton<IPathValidator, PathValidator>();

            return services;
        }

    }
}
