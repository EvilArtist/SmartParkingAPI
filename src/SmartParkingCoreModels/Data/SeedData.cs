using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartParkingCoreModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Data
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging()
                .AddDbContext<ApplicationDbContext>(options => 
                    options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName)));
            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            MigrateApplicationDb(scope);
        }

        private static void MigrateApplicationDb(IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();
            EnsureSeedData(context);
        }

        public static void EnsureSeedData(ApplicationDbContext dbContext)
        {
            if (!dbContext.CameraProtocolType.Any())
            {
                SeedConfigurationTypeData(dbContext);
            }
            dbContext.SaveChanges();
        }

        private static void SeedConfigurationTypeData(ApplicationDbContext dbContext)
        {
            _ = dbContext.CameraProtocolType.Add(new CameraProtocolType
            {
                ProtocolName = "http",
                Url = "https://en.wikipedia.org/wiki/Computer-generated_imagery"
            });
            _ = dbContext.CameraProtocolType.Add(new CameraProtocolType
            {
                ProtocolName = "rtsp",
                Url = "https://en.wikipedia.org/wiki/Real_Time_Streaming_Protocol"
            });

        }
    }
}
