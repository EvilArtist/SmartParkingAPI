using SmartParking.Share.Constants;
using SmartParkingCoreModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Parking.PriceBooks
{
    public abstract class PriceBookCondition : MultiTanentModel
    {
        [MaxLength(EntityConstants.NameMaxLength)]
        public virtual string Name { get; set; }
        [MaxLength(EntityConstants.DescriptionMaxLength)]
        public virtual string Condition { get; set; }
        [NotMapped]
        public virtual PriceCondition PriceConditionType { get; }       
    }

    public class PriceListDefaultCondition : PriceBookCondition
    {
        [NotMapped]
        public override PriceCondition PriceConditionType => PriceCondition.Default;
    }

    public class PriceListWeekdayCondition : PriceBookCondition
    {
        [NotMapped]
        public override PriceCondition PriceConditionType => PriceCondition.Weekday;

        [NotMapped]
        public DayOfWeek[] Days {
            get {
                string[] days = Condition.Split(',');
                return days.Select(x => Enum.Parse<DayOfWeek>(x)).ToArray();
            }
            set {
                Condition = string.Join(", ", value);
            }
        }
    }

    public class PriceListHollidayCondition : PriceBookCondition
    {
        [NotMapped]
        public override PriceCondition PriceConditionType => PriceCondition.Holliday;

        public override string Condition {
            get => string.IsNullOrEmpty(base.Condition) ? "Ngày lễ" : base.Condition.Trim();
            set => base.Condition = value; 
        }

        public PriceListHollidayCondition()
        {
            Condition = "Ngày lễ";
        }
    }

    public class PriceListDurationCondition : PriceBookCondition
    {
        [NotMapped]
        public override PriceCondition PriceConditionType => PriceCondition.Duration;
        public override string Condition {
            get {
                return StartDate.ToString("MM/dd/yyyy") + " - " + EndDate.ToString("MM/dd/yyyy");
            }
            set {
                Regex r = new(@"(\d+\/\d+\/\d+)\s*-\s*(\d+\/\d+\/\d+)");
                var matches = r.Match(value);
                if (matches.Success)
                {
                    var canParseStartDate = DateTime.TryParse(matches.Groups[1].Value, out DateTime startDate);
                    var canParseEndDate = DateTime.TryParse(matches.Groups[2].Value, out DateTime endDate);
                    if(canParseStartDate && canParseEndDate)
                    {
                        StartDate = startDate;
                        EndDate = endDate;
                    }
                }
            }
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
