using System;

namespace SmartParkingCoreModels.Common
{
    public class MultiTanentModel : IMultiTanentModel
    {
        public virtual Guid Id { get; set; }
        public virtual string ClientId { get; set; }
    }
}
