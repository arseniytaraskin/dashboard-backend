using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Orleans;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Configuration;

namespace JumpStartOrleans.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Host.UseOrleansClient((context, client) =>
            {
                client.UseLocalhostClustering(); 

                
                client.Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";  
                    options.ServiceId = "YourServiceId";  
                });
            });

            
            builder.Services.AddControllers();

            var app = builder.Build();

            
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
            app.Run();
        }
    }
}
