using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartParking.Share.Constants;
using SmartParkingAbstract.Services.Parking;
using SmartParkingAbstract.ViewModels.Parking.SerialPortConfiguration;
using SmartParkingCoreModels.Data;
using SmartParkingCoreModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreServices.Parking
{
    public class SerialPortService : ISerialPortService
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;

        public SerialPortService(IMapper mapper, ApplicationDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public async Task<SerialPortConfigViewModel> CreateConfig(SerialPortConfigViewModel configurationViewModel)
        {
            var serialPort = mapper.Map<SerialPortConfiguration>(configurationViewModel);
            var result = await dbContext.AddAsync(serialPort);
            await dbContext.SaveChangesAsync();
            return mapper.Map<SerialPortConfigViewModel>(result.Entity);
        }

        public async Task<SerialPortConfigViewModel> GetSerialPortConfigById(Guid id, string clientId)
        {
            var serialPort = await dbContext.SerialPortConfigurations.Where(x => x.ClientId == clientId
                && x.Id == id
            ).FirstOrDefaultAsync();
            return mapper.Map<SerialPortConfigViewModel>(serialPort) ;
        }

        public async Task<IEnumerable<SerialPortConfigViewModel>> SearchSerialPortConfig(string clienId, DeviceStatus? status)
        {
            var query = dbContext.SerialPortConfigurations.Where(x => x.ClientId == clienId);
            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }
            var serialPorts = await query.ToListAsync();
            return mapper.Map<List<SerialPortConfiguration>, List<SerialPortConfigViewModel>>(serialPorts);
        }
    }
}
