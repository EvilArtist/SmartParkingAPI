using SmartParking.Share.Constants;
using SmartParkingCoreModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Parking.PriceBooks
{
    public class PriceList : MultiTanentModel
    {
        [MaxLength(EntityConstants.NameMaxLength)]
        public string Name { get; set; }
        public Guid PriceBookId { get; set; }
        public PriceBook PriceBook { get; set; }

        /// <summary>
        /// Thời gian bắt đầu tính
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Thời gian kết thúc tính
        /// </summary>
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// Cho phép vượt khung start + offset -> end + offset
        /// </summary>
        public TimeSpan Offset { get; set; }

        /// <summary>
        /// Qua đêm
        /// </summary>
        [NotMapped]
        public virtual bool Overnight => EndTime < StartTime;

        /// <summary>
        /// Cả ngày
        /// </summary>
        [NotMapped]
        public virtual bool FullDay => EndTime == StartTime;

        public PriceCalculation Calculation { get; set; }
    }
}
