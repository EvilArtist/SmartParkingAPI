﻿using SmartParking.Share.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Parking
{
    public class CardStatus
    {
        public Guid Id { get; set; }
        [MaxLength(EntityConstants.NameMaxLength)]
        public string Name { get; set; }
        [MaxLength(EntityConstants.ShortDescriptionMaxLength)]
        public string Description { get; set; }
        [MaxLength(EntityConstants.CodeMaxLength)]
        public string Code { get; set; }
    }
}