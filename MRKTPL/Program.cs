using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MRKTPL.Common;
using System;

namespace MRKTPL
{
    public class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void Main(string[] args)
        {
            //CreateWebHostBuilder(args).Build().Run();
            IWebHost host = BuildWebHost(args);
​
            using (IServiceScope scope = host.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                try
                {
                    IServiceProvider serviceProvider = services.GetRequiredService<IServiceProvider>();
                    IConfiguration configuration = services.GetRequiredService<IConfiguration>();
                    AuthorizationPolicy.CreateRoles(serviceProvider, configuration).Wait();

                }
                catch (Exception exception)
                {
                    ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(exception, "An error occurred while creating roles");
                }
            }
​
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>()
               .Build();
        }
    }

    //public static IWebHostBuilder CreateWebHostBuilder(string[] args)
    //    {
    //        return WebHost.CreateDefaultBuilder(args)
    //           .UseStartup<Startup>();
    //    }
    //}
}
