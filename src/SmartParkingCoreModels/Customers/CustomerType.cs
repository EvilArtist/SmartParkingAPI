using SmartParking.Share.Constants;
using SmartParkingCoreModels.Common;
using System.ComponentModel.DataAnnotations;

namespace SmartParkingCoreModels.Customers
{
    public class CustomerType: IMultiTanentModel
    {
        [Key]
        [MaxLength(EntityConstants.CodeMaxLength)]
        public string Code { get; set; }
        [MaxLength(EntityConstants.NameMaxLength)]
        public string Name { get; set; }
        [MaxLength(EntityConstants.DescriptionMaxLength)]
        public string Description { get; set; }
        public string ClientId { get; set; }
    }

}
