using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ApplicationCore.Entities.OrderAggregate;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Starcounter.Nova;
using Starcounter.Nova.Abstractions;
using Starcounter.Nova.AspNetCore;
using Starcounter.Nova.Hosting;

namespace Microsoft.eShopWeb
{
    public class Program
    {
        private const string DatabaseName = "nova";

        public static void PrintTableHierarchy()
        {
            Db.Transact(() => {
                PrintTablesThatInherit(null, 0);
            });
        }

        public static void PrintTablesThatInherit(Starcounter.Metadata.Table parent, int level)
        {
            string indent = (new StringBuilder(level).Insert(0, " ", level * 2)).ToString();
            var result = Db.SQL<Starcounter.Metadata.Table>("SELECT t FROM Starcounter.Metadata.Table t");
            foreach (var t in result)
            {
                if (object.Equals(t.Inherits, parent))
                {
                    Console.Write("{0}{1}", indent, t.Name);
                    Console.WriteLine();
                    PrintTablesThatInherit(t, level + 1);
                }
            }
        }

        public static void Main(string[] args)
        {
            Process.Start("staradmin.exe", "kill devall").WaitForExit();
            if (Directory.Exists(DatabaseName))
            {
                Directory.Delete(DatabaseName, true);
            }

            var sw = new Stopwatch();
            sw.Start();
            if (!Starcounter.Nova.Options.StarcounterOptions.TryOpenExisting(DatabaseName))
            {
                Directory.CreateDirectory(DatabaseName);
                Starcounter.Nova.Bluestar.ScCreateDb.Execute(DatabaseName);
            }

            var tryOpenExistingTime = sw.Elapsed;
            sw.Restart();
            using (var appHost = new AppHostBuilder()
                .UseDatabase(DatabaseName)
                .Build())
            {
                var host = BuildWebHost(args, appHost);
                var startHostTime = sw.Elapsed;
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogInformation("Started with Starcounter");

                    try
                    {
                        sw.Restart();
                        Mapper.Initialize(config => {
                            foreach (var typeInfo in typeof(Program)
                                .Assembly
                                .DefinedTypes
                                .Where(info => info.BaseType == typeof(BaseEntity)))
                            {
                                config.CreateMap(typeInfo.AsType(), typeInfo.AsType());
                            }
                        });
                        var mapperInitiizeTime = sw.Elapsed;
                        var catalogContextSeed = new CatalogContextSeed();
                        sw.Restart();
                        Db.Transact(() => catalogContextSeed.SeedStarcounter(loggerFactory));
                        var seedTime = sw.Elapsed;
                        logger.LogInformation($"TryOpenExisting: {tryOpenExistingTime}, StartHost: {startHostTime}, Mapper: {mapperInitiizeTime}, Seed: {seedTime}");
//                        var catalogContext = services.GetRequiredService<CatalogContext>();
                        //                        CatalogContextSeed.SeedAsync(catalogContext, loggerFactory)
                        //                            .Wait();

                        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                        AppIdentityDbContextSeed.SeedAsync(userManager).Wait();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the DB.");
                    }
                }

                host.Run();
            }

        }

        public static IWebHost BuildWebHost(string[] args, IAppHost appHost) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://0.0.0.0:5106")
                .UseStartup<Startup>()
                .ConfigureServices(services => { services.AddStarcounter(appHost); })
                .Build();
    }
}
