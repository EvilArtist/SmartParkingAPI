using SmartParking.Share.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartParkingCoreModels.Parking
{
    public class CameraProtocolType
    {
        public Guid Id { get; set; }
        [MaxLength(EntityConstants.ShortNameMaxLength)]
        public string ProtocolName { get; set; }
        [MaxLength(EntityConstants.UrlMaxLength)]
        public string Url { get; set; }
    }
}
