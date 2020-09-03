using B2DriverLicense.Core.EF;
using B2DriverLicense.Core.Entities;
using B2DriverLicense.Service;
using B2DriverLicense.Service.Repositories;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace B2DriverLicense.AppCrawler
{
    public class Program
    {
        private static IServiceProvider _serviceProvider;

        private static void RegisterServices()
        {
            var services = new ServiceCollection();

            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();
           
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IQuestionRepository, QuestionRepository>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ConsoleApplication>();
            _serviceProvider = services.BuildServiceProvider(true);
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }

        static async Task Main(string[] args)
        {
            RegisterServices();
            IServiceScope scope = _serviceProvider.CreateScope();
            await scope.ServiceProvider.GetRequiredService<ConsoleApplication>().Run();
            DisposeServices();
        }
    }  
}
