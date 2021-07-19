using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartParking.Share.Constants;
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
        public static void EnsureSeedData(string connectionString, string assemblyName)
        {
            var services = new ServiceCollection();
            services.AddLogging()
                .AddDbContext<ApplicationDbContext>(options => 
                    options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(assemblyName)));
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
            SeedCardStatusData(dbContext);
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

        private static void SeedCardStatusData(ApplicationDbContext dbContext)
        {
            if (!dbContext.CardStatuses.Any())
            {
                dbContext.CardStatuses.AddRange(new CardStatus[]
                {

                    new CardStatus
                    {
                         Name = "Hoạt động",
                         Code = CardStatusCode.Active,
                         Description = "Thẻ mới tạo, hoặc thẻ vãng lai",
                    },
                    new CardStatus
                    {
                         Name = "Trong bãi xe",
                         Code = CardStatusCode.Parking,
                         Description = "Khi có khách đang sử dụng",
                    },
                    new CardStatus
                    {
                         Name = "Ngoài bãi xe",
                         Code = CardStatusCode.Checkout,
                         Description = "Thẻ khách hàng, đã được mang ra ngoài",
                    },
                    new CardStatus
                    {
                         Name = "Khoá",
                         Code = CardStatusCode.Lock,
                         Description = "Thẻ báo mất, hoặc mất xe",
                    },
                });
            }
        }
    }
}
