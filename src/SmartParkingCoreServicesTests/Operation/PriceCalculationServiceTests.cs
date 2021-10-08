using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartParkingAbstract.ViewModels.Parking.PriceBook;
using SmartParkingCoreModels.Data;
using System;
using System.Threading.Tasks;

namespace SmartParkingCoreServices.Operation.Tests
{
    [TestClass()]
    public class PriceCalculationServiceTests
    {
        const string CONNECTION_STRING = "Data Source=DESKTOP-T6FS6MS;database=SmartParking.Db;trusted_connection=yes;";
        [TestMethod()]
        public async Task CalculateTest()
        {
            try
            {
                DbContextOptionsBuilder<ApplicationDbContext> contextBuilder = new();
                contextBuilder.UseSqlServer(CONNECTION_STRING);
                ApplicationDbContext dbContext = new(contextBuilder.Options, null);
                PriceCalculationService service = new PriceCalculationService(dbContext);
                PriceCalculationParam calculationParam = new()
                {
                    CheckinTime = DateTime.Today + TimeSpan.FromHours(-5),
                    CheckoutTime = DateTime.Today + TimeSpan.FromHours(7),
                    VehicleTypeId = Guid.Parse("1edf20cf-935b-4909-bea1-08d947b25388"),
                    SubscriptionTypeId = Guid.Parse("d30f974b-9aad-460b-8f95-08d9499588a3"),
                };
                var priceList = await service.Calculate(calculationParam);
                var total = service.GetTotal(priceList);
                Assert.AreEqual(total, 150);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public async Task CalculateTestInvalidSubscription()
        {
            try
            {
                DbContextOptionsBuilder<ApplicationDbContext> contextBuilder = new();
                contextBuilder.UseSqlServer(CONNECTION_STRING);
                ApplicationDbContext dbContext = new ApplicationDbContext(contextBuilder.Options, null);
                PriceCalculationService service = new PriceCalculationService(dbContext);
                PriceCalculationParam calculationParam = new()
                {
                    CheckinTime = DateTime.Today + TimeSpan.FromHours(-5),
                    CheckoutTime = DateTime.Today + TimeSpan.FromHours(7),
                    VehicleTypeId = Guid.Parse("8B27DCD7-A645-4315-BEA2-08D947B25388"),
                    SubscriptionTypeId = Guid.Parse("d30f974b-9aad-460b-8f95-08d9499588a3"),
                };
                var priceList = await service.Calculate(calculationParam);
                var total = service.GetTotal(priceList);
                Assert.AreEqual(total, 0);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public async Task CalculateTestOneDayTwoTime()
        {
            try
            {
                DbContextOptionsBuilder<ApplicationDbContext> contextBuilder = new();
                contextBuilder.UseSqlServer(CONNECTION_STRING);
                ApplicationDbContext dbContext = new ApplicationDbContext(contextBuilder.Options, null);
                PriceCalculationService service = new PriceCalculationService(dbContext);
                PriceCalculationParam calculationParam = new()
                {
                    CheckinTime = DateTime.Today + TimeSpan.FromHours(7),
                    CheckoutTime = DateTime.Today + TimeSpan.FromHours(20),
                    VehicleTypeId = Guid.Parse("f418388c-6b96-4f0e-bea5-08d947b25388"),
                    SubscriptionTypeId = Guid.Parse("d30f974b-9aad-460b-8f95-08d9499588a3"),
                };
                var priceList = await service.Calculate(calculationParam);
                var total = service.GetTotal(priceList);
                Assert.AreEqual(total, 17000);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public async Task CalculateTestOvernight()
        {
            try
            {
                DbContextOptionsBuilder<ApplicationDbContext> contextBuilder = new();
                contextBuilder.UseSqlServer(CONNECTION_STRING);
                ApplicationDbContext dbContext = new ApplicationDbContext(contextBuilder.Options, null);
                PriceCalculationService service = new PriceCalculationService(dbContext);
                PriceCalculationParam calculationParam = new()
                {
                    CheckinTime = DateTime.Today + TimeSpan.FromHours(20),
                    CheckoutTime = DateTime.Today.AddDays(1) + TimeSpan.FromHours(4),
                    VehicleTypeId = Guid.Parse("f418388c-6b96-4f0e-bea5-08d947b25388"),
                    SubscriptionTypeId = Guid.Parse("d30f974b-9aad-460b-8f95-08d9499588a3"),
                };
                var priceList = await service.Calculate(calculationParam);
                var total = service.GetTotal(priceList);
                Assert.AreEqual(total, 13000);
            }
            catch
            {
                Assert.Fail();
            }
        }

    }
}