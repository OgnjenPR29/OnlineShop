using DataLayer.Models;
using ServiceLayer.DataBase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ServiceLayer
{
    public class Mapping : Profile
    {
		public Mapping()
		{
			MapAuth();

			MapUser();

			MapArticle();

			MapOrder();
		}

		public void MapAuth()
		{
			CreateMap<User, RegDto>().ReverseMap();

			CreateMap<Admin, RegDto>().ReverseMap();

			CreateMap<Shopper, RegDto>().ReverseMap();

			CreateMap<Salesman, RegDto>().ReverseMap();
		}

		public void MapUser()
		{
		}

		public void MapArticle()
		{
		}

		public void MapOrder()
		{
			
		}
	}
}
