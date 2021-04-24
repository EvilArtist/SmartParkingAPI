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
        public ApplicationDbContext(DbContextOptions<ApplicationIdentityContext> options)
                : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
