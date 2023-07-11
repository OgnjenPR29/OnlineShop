using DataLayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    class Shopper : User, IShopper
    {
        public ICollection<Order> Orders { get; set; }

    }
}
