using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartParking.Share.Constants;
using SmartParking.Share.Extensions;
using SmartParkingAbstract.Services.Parking;
using SmartParkingAbstract.ViewModels.DataImport;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking;
using SmartParkingCoreModels.Customers;
using SmartParkingCoreModels.Data;
using SmartParkingCoreModels.Parking;
using SmartParkingCoreServices.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreServices.Parking
{
    public class CardService : MultitanentService, ICardService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public CardService(ApplicationDbContext dbContext,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor): base(httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<CardViewModel> CreateCard(CreateCardViewModel model)
        {
            Card newCard = mapper.Map<Card>(model);
            var cards = await dbContext.Cards
                .Where(x => x.ClientId == ClientId && (x.IdentityCode == model.IdentityCode || x.Name == model.Name))
                .ToListAsync();
            bool isCardAdded = cards.Any(x => x.IdentityCode == model.IdentityCode);
            if (isCardAdded)
            {
                throw new Exception("Thẻ đã được thêm");
            }
            if (cards.Count > 0)
            {
                throw new InvalidOperationException($"Đã có thẻ khác được gán tên {model.Name}");
            }
            
            newCard.StatusCode = CardStatusCode.Active;
            var result = await dbContext.AddAsync(newCard);
            await dbContext.SaveChangesAsync();
            dbContext.Entry(result.Entity).Reference(x => x.Subscription.SubscriptionType).Load();
            dbContext.Entry(result.Entity).Reference(x => x.VehicleType).Load();
            return mapper.Map<CardViewModel>(result.Entity);
        }

        public async Task<CardViewModel> GetCardById(Guid cardId)
        {
            var result = await dbContext.Cards
                .Include(x => x.Status)
                .Include(x => x.VehicleType)
                .Include(x => x.Subscription)
                .ThenInclude(x => x.SubscriptionType)
                .Where(x => x.Id == cardId && x.ClientId == ClientId)
                .FirstOrDefaultAsync();
            return mapper.Map<CardViewModel>(result);
        }

        public async Task<CardViewModel> GetCardByCode(string code)
        {
            var result = await dbContext.Cards
                .Include(x => x.Status)
                .Include(x => x.VehicleType)
                .Include(x => x.Subscription)
                .ThenInclude(x => x.SubscriptionType)
                .Where(x => x.IdentityCode == code && x.ClientId == ClientId)
                .FirstOrDefaultAsync();
            return mapper.Map<CardViewModel>(result);
        }

        public async Task<CardViewModel> GetCardByName(string cardName)
        {
            var result = await dbContext.Cards
                .Include(x => x.Status)
                .Include(x => x.VehicleType)
                .Include(x => x.Subscription)
                .ThenInclude(x => x.SubscriptionType)
                .Where(x => x.Name == cardName && x.ClientId == ClientId)
                .FirstOrDefaultAsync();
            return mapper.Map<CardViewModel>(result);
        }

        public async Task<QueryResultModel<CardViewModel>> GetCards(QueryModel queryModel)
        {
            var data = dbContext.Cards
                .Include(x=>x.Status)
                .Include(x=>x.VehicleType)
                .Include(x=>x.Subscription)
                .ThenInclude(x=>x.SubscriptionType)
                .Include(x=>x.Subscription)
                .ThenInclude(x=>x.Vehicle)
                .Where(x => x.ClientId == ClientId);
            if(!string.IsNullOrEmpty(queryModel.QueryString))
            {
                data = data.Where(x => x.Name.Contains(queryModel.QueryString));
            }

            foreach (var refinement in queryModel.Refinements)
            {
                if (refinement.Key == CardRefinements.ParkingId)
                {
                    var parkingId = Guid.Parse(refinement.Value);
                    var parkingAssignment = dbContext.CardParkingAssignments
                        .Where(x => x.ParkingId == parkingId)
                        .Select(x => x.CardId)
                        .Distinct();
                    data = data.Join(parkingAssignment, x => x.Id, y => y, (x, y) => x);
                }

                if (refinement.Key == CardRefinements.NotAssignedToParkingId)
                {
                    var parkingId = Guid.Parse(refinement.Value);
                    var parkingAssignment = dbContext.CardParkingAssignments
                        .Where(x => x.ParkingId == parkingId)
                        .Select(x => x.CardId)
                        .Distinct();
                }

                if (refinement.Key == CardRefinements.CardStatus)
                {
                    var cardStatus = refinement.Value;

                    data = data.Where(x => x.Status.Code == cardStatus);
                }
            }
            var totalCount = await data.CountAsync();
            var result = await data
                .Include(x=>x.Status)
                .Include(x=>x.Subscription)
                .ThenInclude(y => y.SubscriptionType)
                .Include(x => x.Subscription)
                .ThenInclude(y=>y.Customer)
                .Include(x=>x.VehicleType)
                .OrderBy(x => x.Name)
                .PagedBy(queryModel.Page, queryModel.PageSize)
                .ToListAsync();
            return new QueryResultModel<CardViewModel>(mapper.Map<IEnumerable<CardViewModel>>(result))
            {
                Page = queryModel.Page,
                TotalCount = totalCount
            };
        }

        public async Task<CardViewModel> UpdateCard(UpdateCardViewModel model)
        {
            var cards = await dbContext.Cards
                .Where(x => x.ClientId == ClientId && (x.Id == model.Id || x.Name == model.Name))
                .ToListAsync();
            if (cards.Count > 1)
            {
                throw new Exception($"Đã có thẻ khác được gán tên {model.Name}");
            }
            var card = cards[0];
            mapper.Map(model, card);
            var result = dbContext.Update(card);
            await dbContext.SaveChangesAsync();

            dbContext.Entry(result.Entity).Reference(x => x.Subscription).Load();
            dbContext.Entry(result.Entity).Reference(x => x.Subscription.SubscriptionType).Load();
            dbContext.Entry(result.Entity).Reference(x => x.VehicleType).Load();
            return mapper.Map<CardViewModel>(result.Entity);
        }

        public async Task<IEnumerable<CardViewModel>> ImportData(IEnumerable<CardDataImport> data)
        {
            var cardCodes = data.Select(x => x.IdentityCode).Distinct();
            var cardNames = data.Select(x => x.Name).Distinct();
            var customersName = data.Select(x => x.CustomerName).Distinct();
            var vehicleTypesName = data.Select(x => x.VehicleTypeName).Distinct();
            var vehicleLicenses = data.Select(x => x.LicensePlate).Distinct();
            var existingCard = await dbContext.Cards
                .Where(x => x.ClientId == ClientId)
                .Where(x => cardCodes.Contains(x.IdentityCode) || cardNames.Contains(x.Name))
                .ToListAsync();
            var subscriptionTypes = await dbContext.SubscriptionTypes.Where(x => x.ClientId == ClientId)
                .ToListAsync();
            var customers = await dbContext.Customers
                .Where(x => x.ClientId == ClientId && customersName.Contains(x.CustomerCode))
                .ToListAsync();
            var vehicleTypes = await dbContext.VehicleTypes
                .Where(x => x.ClientId == ClientId && vehicleTypesName.Contains(x.Name))
                .ToListAsync();
            var vehicles = await dbContext.Vehicles
                .Where(x => ClientId == x.ClientId && vehicleLicenses.Contains(x.LicensePlate))
                .ToListAsync();
            Func<CardDataImport, Tuple<bool, string>> validateCardItem = (cardItem) =>
            {
                var checkRequired = !string.IsNullOrEmpty(cardItem.CustomerName) &&
                    !string.IsNullOrEmpty(cardItem.VehicleTypeName) &&
                    !string.IsNullOrEmpty(cardItem.SubscriptionTypeName) && 
                    !string.IsNullOrEmpty(cardItem.LicensePlate);
                if (!checkRequired)
                {
                    return new Tuple<bool, string>(false, "Thiếu mã khách hàng, loại vé, loại xe hoặc biển số");
                }
                var validateCustomer = customers.Any(x => x.CustomerCode == cardItem.CustomerName);
                if (!validateCustomer)
                {
                    return new Tuple<bool, string>(false, $"Khách hàng ${cardItem.CustomerName} không tồn tại");
                }
                var validateVehicleType = vehicleTypes.Any(x => x.Name == cardItem.VehicleTypeName);
                if (!validateVehicleType)
                {
                    return new Tuple<bool, string>(false, $"Loại xe ${cardItem.VehicleTypeName} không tồn tại");
                }
                var validateSubscriptionType = subscriptionTypes.Any(x => x.Name == cardItem.SubscriptionTypeName);
                if (!validateSubscriptionType)
                {
                    return new Tuple<bool, string>(false, $"Loại thuê bao ${cardItem.SubscriptionTypeName} không tồn tại");
                }
                var validateVehicle = !vehicles.Any(x => x.LicensePlate == cardItem.LicensePlate);
                if (!validateVehicle)
                {
                    return new Tuple<bool, string>(false, $"Xe có biển số ${cardItem.SubscriptionTypeName} đã được đăng ký");
                }
                return new Tuple<bool, string>(true, string.Empty);
            };
            List<Card> ImportCards = new();
            foreach (var item in data)
            {
                try
                {
                    var cards = existingCard.Where(x => x.IdentityCode == item.IdentityCode || x.Name == item.Name).ToList();
                    if (cards.Count > 1)
                    {
                        throw new Exception($"Đã có thẻ khác được gán tên {item.Name}");
                    }
                    Card card;
                    if (cards.Count == 1)
                    {
                        card = cards[0];
                        card.Name = item.Name;
                        if (card.StatusCode != CardStatusCode.Active && card.StatusCode != CardStatusCode.Checkout)
                        {
                            throw new Exception($"Không thể cập nhật thẻ {card.Name} khi đang sử dụng");
                        }
                        if (card.SubscriptionId == null)
                        {
                            var validation = validateCardItem(item);
                            if (validation.Item1)
                            {
                                var vehicleType = vehicleTypes.First(x => x.Name == item.VehicleTypeName);
                                var customer = customers.First(x => x.CustomerCode == item.CustomerName);
                                var subscriptionType = subscriptionTypes.First(x => x.Name == item.SubscriptionTypeName);
                                card.VehicleTypeId = vehicleType.Id;
                                Vehicle vehicle = new()
                                {
                                    LicensePlate = item.LicensePlate,
                                    VehicleTypeId = vehicleType.Id
                                };
                                Subscription subscription = new()
                                {
                                    CustomerId = customer.Id,
                                    SubscriptionTypeId = subscriptionType.Id,
                                    Vehicle = vehicle
                                };
                                //await dbContext.AddAsync(subscription);
                                card.Subscription = subscription;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine(validation.Item2);
                                Console.ForegroundColor = ConsoleColor.Yellow;
                            }
                        }
                        else
                        {
                            card.SubscriptionId = null;
                        }
                        var result = dbContext.Update(card);
                        ImportCards.Add(result.Entity);
                    }
                    else
                    {
                        card = new()
                        {
                            Name = item.Name,
                            StatusCode = CardStatusCode.Active,
                            IdentityCode = item.IdentityCode,

                        };
                        var vehicleType = vehicleTypes.FirstOrDefault(x => x.Name == item.VehicleTypeName);
                        var validation = validateCardItem(item);
                        if (validation.Item1)
                        {
                            var customer = customers.First(x => x.CustomerCode == item.CustomerName);
                            var subscriptionType = subscriptionTypes.First(x => x.Name == item.SubscriptionTypeName);
                            card.VehicleTypeId = vehicleType.Id;
                            Vehicle vehicle = new()
                            {
                                LicensePlate = item.LicensePlate,
                                VehicleTypeId = vehicleType.Id
                            };
                            Subscription subscription = new()
                            {
                                CustomerId = customer.Id,
                                SubscriptionTypeId = subscriptionType.Id,
                                Vehicle = vehicle
                            };
                            card.Subscription = subscription;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(validation.Item2);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }

                        if (vehicleType != null)
                        {
                            card.VehicleTypeId = vehicleType.Id;
                            var result = await dbContext.AddAsync(card);
                            ImportCards.Add(result.Entity);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"Thiếu loại xe cho thẻ {item.Name}");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                    }
                }
                catch
                {

                }
            }

            await dbContext.SaveChangesAsync();

            return mapper.Map<IEnumerable<CardViewModel>>(ImportCards);
        }
    }
}
