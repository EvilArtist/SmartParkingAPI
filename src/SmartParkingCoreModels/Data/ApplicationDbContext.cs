using Microsoft.EntityFrameworkCore;
using SmartParkingCoreModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Data
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<SerialPortConfiguration> SerialPortConfigurations { get; set; }
        public DbSet<CameraConfiguration> CameraConfigurations { get; set; }
        public DbSet<CameraProtocolType> CameraProtocolType { get; set; }
        public DbSet<SlotType> SlotTypes { get; set; }
        public DbSet<SlotTypeConfiguration> SlotTypeConfigurations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
        {
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
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
