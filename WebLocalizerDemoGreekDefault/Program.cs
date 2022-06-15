/*
 * Copyright Code Solidi Ltd. (c) 2021, 2022. All rights reserved.
 */

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WebLocalizerDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}