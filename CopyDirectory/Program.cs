using Microsoft.Extensions.DependencyInjection;
using Services;
using System.Threading.Tasks;

namespace CopyDirectory
{
    class Program
    {
        static async Task Main(string[] args)
        {

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
