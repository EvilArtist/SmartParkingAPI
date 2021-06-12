﻿using SmartParkingAbstract.ViewModels.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.Parking.SlotType
{
    public class SlotTypeViewModel: MutiTanentModel
    {
        public string SlotName { get; set; }
        public string Description { get; set; }
    }

    public class SlotTypeCreateViewModel
    {
        public string SlotName { get; set; }
        public string Description { get; set; }
    }
}