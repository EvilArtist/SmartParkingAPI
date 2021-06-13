using SmartParking.Share.Constants;
using SmartParkingCoreModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Parking
{
    public class CameraConfiguration : AuditModel
    {
        [MaxLength(EntityConstants.NameMaxLength)]
        public string CameraName { get; set; }
        [MaxLength(EntityConstants.NameMaxLength)]
        public string ServerName { get; set; }
        [MaxLength(EntityConstants.NameMaxLength)]
        public string UserName { get; set; }
        [MaxLength(EntityConstants.NameMaxLength)]
        public string Password { get; set; }
        [MaxLength(EntityConstants.UrlMaxLength)]
        public string URLTemplate { get; set; }
        public int CameraId { get; set; }
        public int StreamId { get; set; }
        public CameraProtocolType Protocol { get; set; }
        public DeviceStatus Status { get; set; }
        [MaxLength(EntityConstants.NameMaxLength)]
        public string Oem { get; set; }
        public Guid ProtocolId { get; set; }
        public Guid? ParkingId { get; set; }
        public ParkingConfig Parking { get; set; }
    }
}
