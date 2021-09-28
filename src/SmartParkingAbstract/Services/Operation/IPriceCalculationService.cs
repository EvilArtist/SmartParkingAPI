using SmartParkingAbstract.ViewModels.Operation;
using SmartParkingAbstract.ViewModels.Parking.PriceBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.Services.Operation
{
    public interface IPriceCalculationService
    {
        Task<IEnumerable<PriceItemViewModel>> Calculate(PriceCalculationParam param);
        decimal GetTotal(IEnumerable<PriceItemViewModel> priceItems);
    }
}
