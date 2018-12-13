using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WpOportunidades
{
    public class Program
    {
        public static void Main(string[] args)
        {

            MainAsync(args).Wait();

        }
        static async Task MainAsync(string[] args)
        {
            var url = await AuxNotStatic.GetInfoMotorAux("WpOportunidades", 1);
            var host = WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls(url.Url)
                //.UseUrls("http://localhost:5000")
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
