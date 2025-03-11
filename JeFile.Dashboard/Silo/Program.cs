using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Configuration;

namespace Silo
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .UseOrleans(siloBuilder =>
                {
                    // Используем локальный кластер
                    siloBuilder.UseLocalhostClustering();

                    // Настройка кластера
                    siloBuilder.Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = "dev";
                        options.ServiceId = "JeFile.Dashboard";
                    });
                })
                .RunConsoleAsync();
        }
    }
}