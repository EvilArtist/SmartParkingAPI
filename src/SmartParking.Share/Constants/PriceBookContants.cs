using System.Collections.Generic;

namespace SmartParking.Share.Constants
{
    public class PriceBookContants
    {
        public static readonly Dictionary<PriceFormular, string> UnitMap = new()
        {
            {PriceFormular.ByDate, "Ngày" },
            {PriceFormular.ByMonth, "Tháng" },
            {PriceFormular.ByTime, "Giờ" },
            {PriceFormular.ByTurn, "Lượt" },
            {PriceFormular.ByWeek, "Tuần" },
        };
    }
   
}
