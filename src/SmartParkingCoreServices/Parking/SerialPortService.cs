using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartParking.Share.Constants;
using SmartParkingAbstract.Services.Parking;
using SmartParkingAbstract.ViewModels.DataImport;
using SmartParkingAbstract.ViewModels.Parking;
using SmartParkingCoreModels.Data;
using SmartParkingCoreModels.Parking;
using SmartParkingCoreServices.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreServices.Parking
{
    public class SerialPortService : MultitanentService, ISerialPortService
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;

        public SerialPortService(IMapper mapper, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
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

        public async Task<SerialPortConfigViewModel> GetSerialPortConfigById(Guid id)
        {
            var serialPort = await dbContext.SerialPortConfigurations.Where(x => x.ClientId == ClientId
                && x.Id == id
            ).FirstOrDefaultAsync();
            return mapper.Map<SerialPortConfigViewModel>(serialPort) ;
        }

        public async Task<IEnumerable<SerialPortConfigViewModel>> SearchSerialPortConfig(DeviceStatus? status)
        {
            var query = dbContext.SerialPortConfigurations.Where(x => x.ClientId == ClientId);
            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }
            var serialPorts = await query.ToListAsync();
            return mapper.Map<List<SerialPortConfiguration>, List<SerialPortConfigViewModel>>(serialPorts);
        }

        public async Task<IEnumerable<SerialPortConfigViewModel>> ImportData(IEnumerable<MultigateDataImport> data)
        {
            var deviceNameList = data.Select(x => x.DeviceName);
            var updateDevices = await dbContext.SerialPortConfigurations
                .Where(x => deviceNameList.Contains(x.DeviceName))
                .ToListAsync();
            foreach (var device in updateDevices)
            {
                var model = data.FirstOrDefault(x => x.DeviceName == device.DeviceName);
                mapper.Map(model, device);
            }
            dbContext.UpdateRange(updateDevices);
            var newDevices= mapper.Map<IEnumerable<MultigateDataImport>, List<SerialPortConfiguration>>
                (data.Where(y => !updateDevices.Any(x => x.DeviceName == y.DeviceName)));

            await dbContext.AddRangeAsync(newDevices);
            await dbContext.SaveChangesAsync();
            return mapper.Map<List<SerialPortConfiguration>, List<SerialPortConfigViewModel>>(newDevices.Union(updateDevices).ToList());
        }
    }
}
