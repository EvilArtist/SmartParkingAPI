using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartParking.Share.Constants;
using SmartParkingCoreModels.Customers;
using SmartParkingCoreModels.Operation;
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
        private readonly string clientId;

        public SeedData(string clientId)
        {
            this.clientId = clientId;
        }
        public void EnsureSeedData(string connectionString, string assemblyName)
        {
            DbContextOptionsBuilder<ApplicationDbContext> builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(assemblyName));

            ApplicationDbContext context = new(builder.Options, null);
            MigrateApplicationDb(context);
        }

        private void MigrateApplicationDb(ApplicationDbContext dbContext)
        {
            dbContext.Database.Migrate();
            EnsureSeedData(dbContext);
            dbContext.SaveChanges();
        }

        public void EnsureSeedData(ApplicationDbContext dbContext)
        {
            SeedConfigurationTypeData(dbContext);
            SeedCardStatusData(dbContext);
            SeedParkingRecordStatus(dbContext);
            SeedCustomerType(dbContext);
            SeedSubscriptionType(dbContext);
        }

        private static void SeedConfigurationTypeData(ApplicationDbContext dbContext)
        {
            if (!dbContext.CameraProtocolType.Any())
            {
                _ = dbContext.CameraProtocolType.Add(new CameraProtocolType
                {
                    ProtocolName = "http",
                    Url = "https://en.wikipedia.org/wiki/Computer-generated_imagery",
                });
                _ = dbContext.CameraProtocolType.Add(new CameraProtocolType
                {
                    ProtocolName = "rtsp",
                    Url = "https://en.wikipedia.org/wiki/Real_Time_Streaming_Protocol"
                });
            }
        }

        private void SeedCardStatusData(ApplicationDbContext dbContext)
        {
            if (!dbContext.CardStatuses.Any())
            {
                var defaultCardStatuses = SystemCardStatus.Defaults.Select(x =>  new CardStatus()
                {
                    Name = x.Name,
                    Code = x.Code,
                    Description = x.Description,
                    ClientId = clientId
                });
                dbContext.CardStatuses.AddRange(defaultCardStatuses);
            }
        }

        private static void SeedParkingRecordStatus(ApplicationDbContext dbContext)
        {
            if (!dbContext.ParkingRecordStatuses.Any())
            {
                var defaultParkingRecordStatuseses = ParkingRecordConstants.SystemStatuses
                    .Select(x => new ParkingRecordStatus()
                    {
                        Code = x.Code,
                        Name = x.Name,
                        Description = x.Description
                    });
                dbContext.ParkingRecordStatuses.AddRange(defaultParkingRecordStatuseses);
            }
        }

        private void SeedCustomerType(ApplicationDbContext dbContext)
        {
            if (!dbContext.CustomerTypes.Any())
            {
                var defaultCustomerTypes = CustomerConstants.DefaultCustomerTypes
                    .Select(x => new CustomerType()
                    {
                        Code = x.Code,
                        Name = x.Name,
                        Description = x.Description,
                        ClientId = clientId
                    });
                dbContext.CustomerTypes.AddRange(defaultCustomerTypes);
            }
        }

        private void SeedSubscriptionType(ApplicationDbContext dbContext)
        {
            if (!dbContext.SubscriptionTypes.Any())
            {
                var defaultCustomerTypes = SystemSubscriptionType.GetAll()
                    .Select(x => new SubscriptionType()
                    {
                        Id = x.Code,
                        Name = x.Name,
                        Description = x.Description,
                        ClientId = clientId
                    });
                dbContext.SubscriptionTypes.AddRange(defaultCustomerTypes);
            }            
        }
    }
}
