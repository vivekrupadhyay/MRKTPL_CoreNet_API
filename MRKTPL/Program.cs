using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;


namespace MRKTPL
{
    public class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
           
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>();
        }
        
    }

   
}

