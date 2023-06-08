﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideShare.Application.UseCases.DTOs.Create
{
    public class CreateRideDto
    {
        public DateTime StartTime { get; set; }
        public int StartCity { get; set; }
        public int DestinationCity { get; set; }
        public int CarId { get; set; }
    }
}
