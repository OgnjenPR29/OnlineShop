﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.Interfaces
{
    public interface IShopper : IUser
    {
        ICollection<Order> Orders { get; set; }

    }
}
