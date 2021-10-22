using SmartParkingAbstract.ViewModels.DataImport;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.Services.Parking
{
    public interface ICardService
    {
        Task<QueryResultModel<CardViewModel>> GetCards(QueryModel query);
        Task<CardViewModel> CreateCard(CreateCardViewModel model);
        Task<CardViewModel> GetCardById(Guid cardId);
        Task<CardViewModel> GetCardByCode(string cardCode);
        Task<CardViewModel> GetCardByName(string cardName);
        Task<CardViewModel> UpdateCard(UpdateCardViewModel model);
        Task<IEnumerable<CardViewModel>> ImportData(IEnumerable<CardDataImport> data);
    }
}
