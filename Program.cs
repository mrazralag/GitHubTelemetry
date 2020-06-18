using GitHubTelemetry.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GitHubTelemetry
{
    class Program
    {      
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("settings.json", optional: true, reloadOnChange: true)
                .Build();

            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(configuration);
            services.AddTransient<ExcelService>();
            services.AddTransient<GitHubService>();
            services.AddTransient<GitHubTelemetry>();
            services.AddTransient<AzureBlobService>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<GitHubTelemetry>().Run();

            Console.ReadLine();
        }
    }
}
