using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartParkingCoreModels.Data;
using SmartParkingCoreServices.Parking.PriceBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreServices.Parking.PriceBooks.Tests
{
    [TestClass()]
    public class PriceBookServiceTests
    {
        [TestMethod()]
        public void CreatePriceBookTest()
        {
            DbContextOptionsBuilder<ApplicationDbContext> contextBuilder = new();
            
        }
    }
}