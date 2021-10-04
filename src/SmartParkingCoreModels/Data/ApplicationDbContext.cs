using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartParkingCoreModels.Common;
using SmartParkingCoreModels.Customers;
using SmartParkingCoreModels.Operation;
using SmartParkingCoreModels.Parking;
using SmartParkingCoreModels.Parking.PriceBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SmartParking.Share.Constants.IdentityConstants;

namespace SmartParkingCoreModels.Data
{
    public class ApplicationDbContext: DbContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public DbSet<SerialPortConfiguration> SerialPortConfigurations { get; set; }
        public DbSet<CameraConfiguration> CameraConfigurations { get; set; }
        public DbSet<CameraProtocolType> CameraProtocolType { get; set; }
        public DbSet<SlotType> SlotTypes { get; set; }
        public DbSet<SlotTypeConfiguration> SlotTypeConfigurations { get; set; }
        public DbSet<ParkingConfig> Parkings{ get; set; }
        public DbSet<ParkingLane> ParkingLanes{ get; set; }
        public DbSet<Card> Cards{ get; set; }
        public DbSet<CardStatus> CardStatuses { get; set; }
        public DbSet<VehicleType> VehicleTypes{ get; set; }
        public DbSet<SubscriptionType> SubscriptionTypes{ get; set; }

        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<PriceListCondition> PriceListConditions { get; set; }
        public DbSet<PriceListDefaultCondition> PriceListDefaultConditions { get; set; }
        public DbSet<PriceListWeekdayCondition> PriceListWeekdayConditions { get; set; }
        public DbSet<PriceListHollidayCondition> PriceListHollidayConditions { get; set; }
        public DbSet<PriceListDurationCondition> PriceListDurationConditions { get; set; }

        public DbSet<CardParkingAssignment> CardParkingAssignments { get; set; }

        public DbSet<ParkingRecord> ParkingRecords { get; set; }
        public DbSet<ParkingRecordStatus> ParkingRecordStatuses { get; set; }

        public DbSet<Customer> Customers{ get; set; }
        public DbSet<Subscription> Subscriptions{ get; set; }
        public DbSet<Vehicle> Vehicles{ get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
                : base(options)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<SlotTypeConfiguration>(config =>
            {
                config.HasKey(cf => new { cf.ParkingId, cf.SlotTypeId });

                config.HasOne(cf => cf.Parking)
                    .WithMany(parking => parking.SlotTypeConfigurations)
                    .HasForeignKey(cf => cf.ParkingId)
                    .IsRequired();

                config.HasOne(cf => cf.SlotType)
                    .WithMany(slotType => slotType.SlotTypeConfigurations)
                    .HasForeignKey(cf => cf.SlotTypeId)
                    .IsRequired();
            });

            builder.Entity<PriceListCondition>()
                .HasDiscriminator<string>("condition_type")
                .HasValue<PriceListCondition>(nameof(PriceListCondition))
                .HasValue<PriceListDefaultCondition>("Default")
                .HasValue<PriceListDurationCondition>("Duration")
                .HasValue<PriceListHollidayCondition>("Holliday")
                .HasValue<PriceListWeekdayCondition>("DayOfWeek");

            builder.Entity<CardParkingAssignment>(config =>
            {
                config.HasKey(cf => new { cf.ParkingId, cf.CardId });

                config.HasOne(cf => cf.Parking)
                    .WithMany(parking => parking.CardAssignments)
                    .HasForeignKey(cf => cf.ParkingId)
                    .IsRequired();

                config.HasOne(cf => cf.Card)
                    .WithMany(slotType => slotType.ParkingAssignments)
                    .HasForeignKey(cf => cf.CardId)
                    .IsRequired();
            });
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries() )
            {
                if (item.State == EntityState.Added && item.Entity is IMultiTanentModel model && model.ClientId == "")
                {
                    model.ClientId = GetClientId();
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected string GetClientId()
        {
            if (httpContextAccessor != null)
            {
                var claim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ClientId);

                var clientId = claim?.Value ?? "";
                return clientId;
            }
            else
            {
                return "";
            }
        }
    }
}
