using DataLayer.Models;
using DataLayer.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repo
{
    public class SalesmanRepo : Generic<Salesman>, ISalesmanRepo
    {
        public SalesmanRepo(OnlineShopContext context) : base(context)
        {
        }
    }
}
