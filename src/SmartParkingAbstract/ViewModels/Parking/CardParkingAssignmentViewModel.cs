using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.Parking
{
    public class CardParkingAssignmentViewModel
    {
        public Guid ParkingId { get; set; }
        public IEnumerable<Guid> CardsId { get; set; }
    }
}
