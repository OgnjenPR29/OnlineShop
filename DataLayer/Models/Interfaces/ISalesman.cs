using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.Interfaces
{
    interface ISalesman : IUser
    {
        ICollection<Article> Articles { get; set; }

    }
}
