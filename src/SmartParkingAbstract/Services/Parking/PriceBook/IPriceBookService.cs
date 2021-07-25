using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking.PriceBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.Services.Parking.PriceBook
{
    public interface IPriceBookService
    {
        IEnumerable<EnumViewModel> GetPriceFomulars();
        IEnumerable<EnumViewModel> GetPriceCondition();
        Task<QueryResultModel<PriceBookViewModel>> GetPriceBooks(PriceListQuery query);
        Task<PriceBookViewModel> CreatePriceBooks(CreateUpdatePriceViewModel model);
        Task<PriceBookViewModel> GetPriceBookById(string clientId, Guid id);
        Task<PriceBookViewModel> UpdatePriceBooks(CreateUpdatePriceViewModel model);
    }
}
