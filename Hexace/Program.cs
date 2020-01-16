using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hexace.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hexace
{
    public class Program
    {
        public static IHost host;
        public static void Main(string[] args)
        {
            host = CreateHostBuilder(args).Build();

            MainLogic mainLogic;

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                mainLogic = new MainLogic(services);
            }
            var inner = Task.Factory.StartNew(() =>  // task for statistics updating in db
            {
                while (true)
                {
                    Thread.Sleep(1000 * 60 * 60);
                    mainLogic.UpdateInfo();
                }
            });
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
