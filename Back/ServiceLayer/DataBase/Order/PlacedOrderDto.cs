using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DataBase.Order
{
    public class PlacedOrderDto : IDTO
    {
        public string Comment { get; set; }

        public string Address { get; set; }

        public ICollection<PlaceItemDto> Items { get; set; }
    }
}
