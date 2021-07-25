using Microsoft.EntityFrameworkCore;
using SmartParking.Share.Constants;
using System.ComponentModel.DataAnnotations;

namespace SmartParkingCoreModels.Parking.PriceBook
{
    [Owned]
    public class PriceCalculation
    {
        public PriceFormular Type { get; set; }
        [MaxLength(EntityConstants.NameMaxLength)]
        public string Name { get; set; }
        public double Price{ get; set; }
        public double HourBlock { get; set; }
    }
}