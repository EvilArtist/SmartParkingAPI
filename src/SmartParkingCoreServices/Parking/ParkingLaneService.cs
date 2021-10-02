using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
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
    public class ParkingLaneService : MultitanentService, IParkingLaneService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public ParkingLaneService(ApplicationDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper): base(httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ParkingLaneViewModel> CreateParkingLane(CreateUpdateParkingLaneViewModel model)
        {
            ParkingLane parkingLane = new()
            {
                ClientId = ClientId,
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

        public async Task<ParkingLaneViewModel> GetParkingLaneById( Guid laneId)
        {
            var parkingLane = await dbContext.ParkingLanes
               .Include(x => x.Cameras)
               .Include(x => x.MutiFunctionGates)
               .Where(x => x.Id == laneId && ClientId == x.ClientId)
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
                .Where(x => x.ParkingId == parkingId && ClientId == x.ClientId)
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
                .Where(x => x.ClientId == ClientId && x.Id == model.Id)
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

        public async Task<ParkingLaneViewModel> DeleteParkingLane(Guid laneId)
        {
            var parkingLane = await dbContext.ParkingLanes
               .Include(x => x.Cameras)
               .Include(x => x.MutiFunctionGates)
               .Where(x => x.Id == laneId && ClientId == x.ClientId)
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

        public async Task<IEnumerable<ParkingLaneViewModel>> ImportData(IEnumerable<ParkingLaneDataImport> data)
        {
            var parkingNames = data.Select(x => x.ParkingName);
            var parkingIds = await dbContext.Parkings
                .Where(x => x.ClientId == ClientId && parkingNames.Contains(x.Name))
                .Select(x => new { x.Name, x.Id }).ToListAsync();
            var parkingLaneViewModels= new List<ParkingLaneViewModel>();
            foreach (var parkingLaneData in data.ToList())
            {
                try
                {
                    //logger.LogInformation($"Tạo làn xe {parkingLaneData.Name}");
                    var parkingId = parkingIds.FirstOrDefault(y => y.Name == parkingLaneData.ParkingName)?.Id ?? null;
                    ParkingLane parkingLane = await dbContext.ParkingLanes
                        .Where(x => x.Name == parkingLaneData.Name && ClientId == ClientId)
                        .FirstOrDefaultAsync();

                    var cameraNames = parkingLaneData.Cameras.Split(",").Distinct().ToList();
                    var cameras = await dbContext.CameraConfigurations
                        .Where(x => x.ClientId == ClientId && cameraNames.Contains(x.CameraName) && x.ParkingLaneId == null)
                        .ToListAsync();
                    if (cameras.Count != cameraNames.Count)
                    {
                        throw new Exception("Invalid Camera Config");
                    }

                    var serialPortNames = parkingLaneData.MultiFunctionGates.Split("|").Distinct().ToList();
                    var serialPorts = await dbContext.SerialPortConfigurations
                        .Where(x => x.ClientId == ClientId && serialPortNames.Contains(x.Name) && x.ParkingLaneId == null)
                        .ToListAsync();
                    if (serialPorts.Count != serialPortNames.Count)
                    {
                        throw new Exception("Invalid Serail Port Config");
                    }

                    EntityEntry<ParkingLane> result;
                    if (parkingLane != null)
                    {
                        result = dbContext.Update(parkingLane);
                    }
                    else
                    {
                        parkingLane = new ParkingLane()
                        {
                            ClientId = ClientId,
                            Name = parkingLaneData.Name,
                            ParkingId = parkingId
                        };
                        result = await dbContext.AddAsync(parkingLane);
                    }
                    foreach (var camera in cameras)
                    {
                        if (cameraNames.Contains(camera.CameraName))
                        {
                            camera.ParkingLaneId = parkingLane.Id;
                        }
                        else
                        {
                            camera.ParkingLaneId = null;
                        }
                    }
                    foreach (var serialPort in serialPorts)
                    {
                        if (serialPortNames.Contains(serialPort.Name))
                        {
                            serialPort.ParkingLaneId = parkingLane.Id;
                        }
                        else
                        {
                            serialPort.ParkingLaneId = null;
                        }
                    }
                    dbContext.UpdateRange(cameras);
                    dbContext.UpdateRange(serialPorts);
                    await dbContext.SaveChangesAsync();
                    parkingLaneViewModels.Add(new ParkingLaneViewModel()
                    {
                        Id = parkingLane.Id,
                        Name = parkingLane.Name,
                        ParkingId = parkingLane.ParkingId.Value,
                        Cameras = mapper.Map<List<CameraConfiguration>, List<CameraConfigurationViewModel>>(parkingLane.Cameras.ToList()),
                        MultiFunctionGates = mapper.Map<List<SerialPortConfiguration>, List<SerialPortConfigViewModel>>(parkingLane.MutiFunctionGates.ToList())
                    });
                    //logger.LogInformation("Thành công ");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    //logger.LogError(e.Message);
                }
            };
            return parkingLaneViewModels;
        }
    }
}
