using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking.PriceBooks;
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
        Task<PriceBookViewModel> CreatePriceBook(CreateUpdatePriceBookViewModel model);
        Task<PriceBookViewModel> GetPriceBookById( Guid id);
        Task<PriceBookViewModel> UpdatePriceBooks(CreateUpdatePriceBookViewModel model);
    }
}
