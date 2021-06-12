using Microsoft.Extensions.DependencyInjection;
using Services;
using System.IO;
using System.Threading.Tasks;

namespace CopyDirectory
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DirectoryInfo di = Directory.CreateDirectory("as");
            DirectoryInfo di1 = Directory.CreateDirectory("c:\\as");
            DirectoryInfo di2 = Directory.CreateDirectory("z:\\as");
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            await serviceProvider.GetService<CopyDirectoryApplication>().Run();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<CopyDirectoryApplication>();
            services.AddScoped<ICopyDirectoryService, CopyDirectoryService>();
            services.AddScoped<IProgressLogger, ProgressLogger>();

            return services;
        }


    }
}
