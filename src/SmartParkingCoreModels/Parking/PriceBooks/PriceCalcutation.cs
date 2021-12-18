using Microsoft.EntityFrameworkCore;
using SmartParking.Share.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartParkingCoreModels.Parking.PriceBooks
{
    [Owned]
    public class PriceCalculation
    {
        public PriceFormular FormularType { get; set; }
        [MaxLength(EntityConstants.NameMaxLength)]

        /// <summary>
        /// Giá trả theo thuê bao
        /// </summary>
        public double SubscriptionPrice{ get; set; }

        /// <summary>
        /// Giá trả mỗi lần gửi
        /// </summary>
        public double PayPrice{ get; set; }
        public double HourBlock { get; set; }

        [NotMapped]
        public bool IsSubscription => FormularType == PriceFormular.Monthly ||
            FormularType == PriceFormular.Quarterly ||
            FormularType == PriceFormular.Annual;
    }
}