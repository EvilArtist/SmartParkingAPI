using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Parking
{
    public class CardParkingAssignment
    {
        public Guid ParkingId { get; set; }
        public ParkingConfig Parking { get; set; }
        public Guid CardId { get; set; }
        public Card Card { get; set; }
    }
}
