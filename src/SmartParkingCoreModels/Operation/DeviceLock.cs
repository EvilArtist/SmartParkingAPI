using SmartParkingCoreModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Operation
{
    public class DeviceLock : IMultiTanentModel
    {
        public Guid Id { get; set; }
        public string ConnectionId { get; set; }
        public string DeviceName { get; set; }
        public DateTime IssueAt { get; set; }
        public string ClientId { get; set; }
    }
}
