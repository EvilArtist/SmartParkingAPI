using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartParking.Share.Constants;
using SmartParkingAbstract.Services.Parking;
using SmartParkingAbstract.ViewModels.Parking;
using SmartParkingCoreModels.Data;
using SmartParkingCoreModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreServices.Parking
{
    public class ParkingLaneService : IParkingLaneService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public ParkingLaneService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task<ParkingLaneViewModel> CreateParkingLane(CreateUpdateParkingLaneViewModel model)
        {
            ParkingLane parkingLane = new()
            {
                ClientId = model.ClientId,
                Name = model.Name,
                ParkingId = model.ParkingId
            };
            var result = await dbContext.AddAsync(parkingLane);
            var cameraIds = model.CameraIds;
            var cameras = await dbContext.CameraConfigurations
                .Where(x => cameraIds.Contains(x.Id) && x.ParkingLaneId == null)
                .ToListAsync();
            if (cameras.Count != cameraIds.Count)
            {
                throw new Exception("Invalid Camera Config");
            }
            var serialPortIds = model.MultiFunctionGateIds;
            var serialPorts = await dbContext.SerialPortConfigurations
                .Where(x => serialPortIds.Contains(x.Id) && x.ParkingLaneId == null)
                .ToListAsync();
            if (serialPorts.Count != serialPortIds.Count)
            {
                throw new Exception("Invalid Serail Port Config");
            }
            foreach (var camera in cameras)
            {
                camera.ParkingLaneId = result.Entity.Id;
                camera.Status = DeviceStatus.Working;
            }
            foreach (var serialPort in serialPorts)
            {
                serialPort.ParkingLaneId = result.Entity.Id;
                serialPort.Status = DeviceStatus.Working;
            }

            dbContext.UpdateRange(cameras);
            dbContext.UpdateRange(serialPorts);
            await dbContext.SaveChangesAsync();
            return new ParkingLaneViewModel()
            {
                Id = result.Entity.Id,
                Name = result.Entity.Name,
                ParkingId = result.Entity.ParkingId.Value,
                Cameras = mapper.Map<List<CameraConfiguration>, List<CameraConfigurationViewModel>>(cameras),
                MultiFunctionGates = mapper.Map<List<SerialPortConfiguration>, List<SerialPortConfigViewModel>>(serialPorts)
            };
        }

        public async Task<ParkingLaneViewModel> GetParkingLaneById(string clientId, Guid laneId)
        {
            var parkingLane = await dbContext.ParkingLanes
               .Include(x => x.Cameras)
               .Include(x => x.MutiFunctionGates)
               .Where(x => x.Id == laneId && clientId == x.ClientId)
               .FirstOrDefaultAsync();

            return new ParkingLaneViewModel
            {
                Id = parkingLane.Id,
                Name = parkingLane.Name,
                ParkingId = parkingLane.ParkingId.Value,
                Cameras = mapper.Map<ICollection<CameraConfiguration>, List<CameraConfigurationViewModel>>(parkingLane.Cameras),
                MultiFunctionGates = mapper.Map<ICollection<SerialPortConfiguration>, List<SerialPortConfigViewModel>>(parkingLane.MutiFunctionGates)
            };
        }

        public async Task<IEnumerable<ParkingLaneViewModel>> GetParkingLanes(Guid parkingId)
        {
            var parkingLanes = await dbContext.ParkingLanes
                .Include(x => x.Cameras)
                .Include(x => x.MutiFunctionGates)
                .Where(x => x.ParkingId == parkingId)
                .ToListAsync();

            return parkingLanes.Select(x => new ParkingLaneViewModel
            {
                Id = x.Id,
                Name = x.Name,
                ParkingId = x.ParkingId.Value,
                Cameras = mapper.Map<ICollection<CameraConfiguration>, List<CameraConfigurationViewModel>>(x.Cameras),
                MultiFunctionGates = mapper.Map<ICollection<SerialPortConfiguration>, List<SerialPortConfigViewModel>>(x.MutiFunctionGates),
            });
        }


        public async Task<ParkingLaneViewModel> UpdateParkingLane(CreateUpdateParkingLaneViewModel model)
        {
            ParkingLane parkingLane = await dbContext.ParkingLanes
                .Where(x => x.ClientId == model.ClientId && x.Id == model.Id)
                .FirstOrDefaultAsync();

            var cameraIds = model.CameraIds;
            var cameras = await dbContext.CameraConfigurations
                .Where(x => cameraIds.Contains(x.Id)|| x.ParkingLaneId == parkingLane.Id)
                .ToListAsync();
            if (cameras.Any(x => x.ParkingLaneId != null && x.ParkingLaneId != parkingLane.Id && x.Status != DeviceStatus.Ready))
            {
                throw new Exception("Invalid Camera Config");
            }

            var serialPortIds = model.MultiFunctionGateIds;
            var serialPorts = await dbContext.SerialPortConfigurations
                .Where(x => serialPortIds.Contains(x.Id) || x.ParkingLaneId == parkingLane.Id)
                .ToListAsync();
            if (serialPorts.Any(x => x.ParkingLaneId != null && x.ParkingLaneId != parkingLane.Id && x.Status != DeviceStatus.Ready))
            {
                throw new Exception("Invalid Serail Port Config");
            }
            foreach (var camera in cameras)
            {
                if (cameraIds.Contains(camera.Id))
                {
                    camera.ParkingLaneId = parkingLane.Id;
                    camera.Status = DeviceStatus.Working;
                } 
                else
                {
                    camera.ParkingLaneId = null;
                    camera.Status = DeviceStatus.Ready;
                }
            }
            foreach (var serialPort in serialPorts)
            {
                if (serialPortIds.Contains(serialPort.Id))
                {
                    serialPort.ParkingLaneId = parkingLane.Id;
                    serialPort.Status = DeviceStatus.Working;
                }
                else
                {
                    serialPort.ParkingLaneId = null;
                    serialPort.Status = DeviceStatus.Ready;
                }
            }
            parkingLane.Name = model.Name;
            dbContext.Update(parkingLane);
            dbContext.UpdateRange(cameras);
            dbContext.UpdateRange(serialPorts);
            await dbContext.SaveChangesAsync();
            return new ParkingLaneViewModel()
            {
                Id = parkingLane.Id,
                Name = parkingLane.Name,
                ParkingId = parkingLane.ParkingId.Value,
                Cameras = mapper.Map<List<CameraConfiguration>, List<CameraConfigurationViewModel>>(cameras.Where(x=>x.ParkingLaneId == parkingLane.Id).ToList()),
                MultiFunctionGates = mapper.Map<List<SerialPortConfiguration>, List<SerialPortConfigViewModel>>(serialPorts.Where(x => x.ParkingLaneId == parkingLane.Id).ToList())
            };
        }

        public async Task<ParkingLaneViewModel> DeleteParkingLane(string clientId, Guid laneId)
        {
            var parkingLane = await dbContext.ParkingLanes
               .Include(x => x.Cameras)
               .Include(x => x.MutiFunctionGates)
               .Where(x => x.Id == laneId && clientId == x.ClientId)
               .FirstOrDefaultAsync();

            foreach (var camera in parkingLane.Cameras)
            {
                camera.ParkingLaneId = null;
                camera.Status = DeviceStatus.Ready;
            }
            foreach (var serialPort in parkingLane.MutiFunctionGates)
            {
                serialPort.ParkingLaneId = null;
                serialPort.Status = DeviceStatus.Ready;
            }

            dbContext.UpdateRange(parkingLane.Cameras);
            dbContext.UpdateRange(parkingLane.MutiFunctionGates);
            dbContext.Remove(parkingLane);
            await dbContext.SaveChangesAsync();
            return new ParkingLaneViewModel()
            {
                Id = parkingLane.Id,
                Name = parkingLane.Name,
                ParkingId = parkingLane.ParkingId.Value,
                Cameras = mapper.Map<List<CameraConfiguration>, List<CameraConfigurationViewModel>>(parkingLane.Cameras.ToList()),
                MultiFunctionGates = mapper.Map<List<SerialPortConfiguration>, List<SerialPortConfigViewModel>>(parkingLane.MutiFunctionGates.ToList())
            };
        }
    }
}
