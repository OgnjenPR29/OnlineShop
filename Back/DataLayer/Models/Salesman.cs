using DataLayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Salesman : User, ISalesman
    {
		public ICollection<Article> Articles { get; set; }
        public Status ApprovalStatus { get ; set ; }
    }
}
