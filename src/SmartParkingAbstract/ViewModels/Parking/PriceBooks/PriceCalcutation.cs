using SmartParking.Share.Constants;
using SmartParkingAbstract.ViewModels.General;

namespace SmartParkingAbstract.ViewModels.Parking.PriceBooks
{
    public class PriceCalculationViewModel
    {
        public PriceFormular FormularType { get; set; }
        public string Name { get; set; }
        public double SubscriptionPrice { get; set; }
        public double PayPrice { get; set; }
        public double HourBlock { get; set; }
        public string Unit { get; set; }
    }

    public class CreatePriceCalcutationViewModel
    {
        public PriceFormular FormularType { get; set; }
        public string Name { get; set; }
        public double SubscriptionPrice { get; set; }
        public double PayPrice { get; set; }
        public double HourBlock { get; set; }
    }

    public class FormularTypeEnumViewModel : EnumViewModel
    {
        public string Unit { get; set; }
    }
}
