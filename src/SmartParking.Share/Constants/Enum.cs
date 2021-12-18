using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Share.Constants
{
    public enum DeviceStatus
    {
        Working,
        Ready,
        Broken
    }

    public enum PriceFormular
    {
        [Description("Theo lượt")]
        ByTurn,
        [Description("Theo giờ")]
        ByTime,
        [Description("Theo ngày")]
        ByDate,
        [Description("Theo tuần")]
        ByWeek,
        [Description("Theo tháng")]
        Monthly,
        [Description("Theo quý")]
        Quarterly,
        [Description("Theo năm")]
        Annual
    }

    public enum PriceCondition
    {
        [Description("Mặc định")]
        Default,
        [Description("Ngày trong tuần")]
        Weekday,
        [Description("Ngày lễ")]
        Holliday,
        [Description("Khoảng thời gian")]
        Duration
    }
}
