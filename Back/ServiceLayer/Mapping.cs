using DataLayer.Models;
using ServiceLayer.DataBase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ServiceLayer.DataBase.User;
using ServiceLayer.DataBase;

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
			CreateMap<User, BasicInfoDto>().ReverseMap();

			CreateMap<Admin, BasicInfoDto>().ReverseMap();

			CreateMap<Shopper, BasicInfoDto>().ReverseMap();

			CreateMap<Salesman, BasicInfoDto>().ReverseMap();

			CreateMap<User, UserDto>().ReverseMap();

			CreateMap<Admin, UserDto>().ReverseMap();

			CreateMap<Shopper, UserDto>().ReverseMap();

			CreateMap<Salesman, UserDto>().ReverseMap();

			CreateMap<Salesman, SalesmanDto>().ReverseMap();
		}

		public void MapArticle()
		{
		}

		public void MapOrder()
		{
			
		}
	}
}
