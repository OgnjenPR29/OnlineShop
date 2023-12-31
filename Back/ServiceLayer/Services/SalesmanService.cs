﻿using AutoMapper;
using DataLayer;
using DataLayer.Models;
using DataLayer.Models.Interfaces;
using ServiceLayer.DataBase.ArticleDto;
using ServiceLayer.DataBase.Auth;
using ServiceLayer.DataBase.Item;
using ServiceLayer.Helpers;
using ServiceLayer.Services.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class SalesmanService : ISalesmanService
    {

		IWorkingRepository workingRepo;

		IMapper _mapper;

		IHelper _helper;

		public SalesmanService(IWorkingRepository workingRepo, IMapper mapper, IHelper helper)
		{ 
			this.workingRepo = workingRepo;
			_mapper = mapper;
			_helper = helper;
		}

		public IServiceOperationResult AddArticle(NewArticleDto articleDto, JwtDto jwtDto)
        {
			IServiceOperationResult operationResult;

			if (articleDto.Quantity < 0)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest, "Kolicina mora biti pozitivan broj!");

				return operationResult;
			}

			ISalesman seller = (ISalesman)_helper.FindUserByJwt(jwtDto.Token);

			if (seller == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Ne postoji prodavac");

				return operationResult;
			}

			if (((Salesman)seller).ApprovalStatus != Status.APPROVED)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Salesman isn't approved!");

				return operationResult;
			}

			if (workingRepo.ArticleRepository.FindFirst(x => x.SalesmanId == seller.Id && x.Name == articleDto.Name) != null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, $"Prodavac ima proizvod sa imenom\"{articleDto.Name}\"!");

				return operationResult;
			}

			Article article = _mapper.Map<Article>(articleDto);
			article.SalesmanId = seller.Id;

			_helper.AddProductImageIfExists(article, articleDto.Image, seller.Id);

			workingRepo.ArticleRepository.Add(article);
			workingRepo.Commit();

			operationResult = new ServiceOperationResult(true);

			return operationResult;
		}

        public IServiceOperationResult DeleteArticle(string articleName, JwtDto jwtDto)
        {
			IServiceOperationResult operationResult;

			ISalesman seller = (ISalesman)_helper.FindUserByJwt(jwtDto.Token);
			if (seller == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller doesn't exist!");

				return operationResult;
			}

			if (((Salesman)seller).ApprovalStatus != Status.APPROVED)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller isn't approved!");

				return operationResult;
			}

			IArticle article = workingRepo.ArticleRepository.FindFirst(x => x.Name == articleName && x.SalesmanId == seller.Id);
			if (article == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "The article doesn't exist!");

				return operationResult;
			}

			workingRepo.ArticleRepository.Remove((Article)article);
			workingRepo.Commit();

			_helper.DeleteArticleProductImageIfExists(article);

			operationResult = new ServiceOperationResult(true);

			return operationResult;
		}

        public IServiceOperationResult GetAllArticles(JwtDto jwtDto) {
			
			IServiceOperationResult operationResult;

			ISalesman salesman = (ISalesman)_helper.FindUserByJwt(jwtDto.Token);

			if(salesman == null)
            {
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Prodavac ne postoji");

				return operationResult;
			}

			if (((Salesman)salesman).ApprovalStatus != Status.APPROVED)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller isn't approved!");

				return operationResult;
			}

			List<IArticle> articles = workingRepo.ArticleRepository.GetAllArticlesFromSeller(salesman.Id).ToList<IArticle>();

			if (articles.Count == 0)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Prodavac nema artikala");

				return operationResult;
			}

			List<ArticleDetailDto> articleDtoList = _helper.ReturnArticlesDetail(articles);

			ArticleListDto response = new ArticleListDto() { Articles = articleDtoList };

			operationResult = new ServiceOperationResult(true, response);

			return operationResult;
		}

        public IServiceOperationResult UpdateArticle(UpdateArticleDto articleDto, JwtDto jwtDto)
        {
			IServiceOperationResult operationResult;

			ISalesman seller = (ISalesman)_helper.FindUserByJwt(jwtDto.Token);
			if (seller == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller doesn't exist!");

				return operationResult;
			}

			if (((Salesman)seller).ApprovalStatus != Status.APPROVED)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller isn't approved!");

				return operationResult;
			}

			IArticle article = workingRepo.ArticleRepository.FindFirst(x => x.SalesmanId == seller.Id && x.Name == articleDto.CurrentName);
			if (article == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound,
					$"Article named \"{articleDto.CurrentName}\" doesn't exist among sellers aricles!");

				return operationResult;
			}

			if (workingRepo.ArticleRepository.FindFirst(x => x.SalesmanId == seller.Id && x.Name == articleDto.NewName && x.Id != article.Id) != null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, $"Seller already has an article named \"{articleDto.NewName}\"!");

				return operationResult;
			}

			if (!string.IsNullOrWhiteSpace(articleDto.NewName))
			{
				article.Name = articleDto.NewName;
				//UpdateProductImagePath(article);
			}

			if (!string.IsNullOrWhiteSpace(articleDto.Description))
			{
				article.Description = articleDto.Description;
			}

			if (articleDto.Quantity >= 0)
			{
				article.Quantity = articleDto.Quantity;
			}

			if (articleDto.Price >= 1)
			{
				article.Price = articleDto.Price;
			}

			workingRepo.ArticleRepository.Update((Article)article);
			workingRepo.Commit();

			operationResult = new ServiceOperationResult(true);

			return operationResult;

		}

		public IServiceOperationResult GetArticleInfo(string articleName, JwtDto jwtDto) {
			IServiceOperationResult operationResult;

			ISalesman salesman = (ISalesman)_helper.FindUserByJwt(jwtDto.Token);

			if (salesman == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Prodavac ne postoji");

				return operationResult;
			}

			if (((Salesman)salesman).ApprovalStatus != Status.APPROVED)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller isn't approved!");

				return operationResult;
			}

			IArticle article = workingRepo.ArticleRepository.FindFirst(article => article.Name == articleName && article.SalesmanId == salesman.Id);

			if (article == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Article not found!");

				return operationResult;
			}

			var image = _helper.GetArticleProductImage(article);

            Console.WriteLine(image);

			ArticleDetailDto articleDto = new ArticleDetailDto(article.Id, article.Name, article.Description, article.Quantity, article.Price, image);

			operationResult = new ServiceOperationResult(true, articleDto);

			return operationResult;

		}
		public IServiceOperationResult GetPreviousOrders(JwtDto jwtDto) {
			
			IServiceOperationResult operationResult;
			
			ISalesman salesman = (ISalesman)_helper.FindUserByJwt(jwtDto.Token);
		
			if(salesman == null)
            {
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller doesn't exist!");

				return operationResult;
            }
			//List<IOrder> orders = workingRepo.OrderRepository.GetAll().ToList<IOrder>();

			/*foreach(Order order in orders) {
				foreach(Item item in order.Items)
                {
					if ((item.Article.SalesmanId == salesman.Id) && IsOrderPending(order))
						orders.Remove(order);
                }
			}*/

			List<IOrder> orders = workingRepo.OrderRepository.ItemsAndArticles(
				order => order.Items.FirstOrDefault(item => item.Article.SalesmanId == salesman.Id) != null).ToList<IOrder>();

			orders.RemoveAll(order => IsOrderPending(order));

			orders.ForEach(order => order.Items = order.Items.ToList().FindAll(item => item.Article.SalesmanId == salesman.Id));

			OrderListDto orderListDto = new OrderListDto();
			orderListDto.Orders = new List<OrderInfoDto>();
			/*{
				Orders = _mapper.Map<List<OrderInfoDto>>(orders)
			};*/

			foreach(var o in orders)
            {
				OrderInfoDto oi = new OrderInfoDto();
				oi.Address = o.Address;
				oi.Comment = o.Comment;
				oi.Created = o.Created;
				oi.Id = o.Id;
				oi.TotalPrice = o.TotalPrice;
				oi.Items = new List<ItemDto>();
				foreach(var i in o.Items)
                {
					ItemDto io = new ItemDto();
					io.ArticleId = i.ArticleId;
					io.ArticleName = i.ArticleName;
					io.PricePerUnit = i.PricePerUnit;
					io.Quantity = i.Quantity;
					byte[] image = _helper.GetArticleProductImage(i.Article);
					io.ArticleImage = image;
					oi.Items.Add(io);
				}
				orderListDto.Orders.Add(oi);
            }

			foreach (var orderDto in orderListDto.Orders)
			{
				IOrder relatedOrder = orders.Find(x => x.Id == orderDto.Id);
				orderDto.RemainingTime = CalculateDeliveryRemainingTime(orderDto.Created, relatedOrder.DeliveryInSeconds);
			}

			operationResult = new ServiceOperationResult(true, orderListDto);

			return operationResult;


		}
		public bool IsOrderPending(IOrder order)
		{
			int secondsPassed = (int)(GetDateTimeAsCEST(DateTime.Now) - order.Created).TotalSeconds;
			if (order.DeliveryInSeconds > secondsPassed)
			{
				return true;
			}

			return false;
		}

		public string CalculateDeliveryRemainingTime(DateTime placedTime, int deliveryTimeInSeconds)
		{
			int secondsPassed = (int)(GetDateTimeAsCEST(DateTime.Now) - placedTime).TotalSeconds;
			int secondsLeft = deliveryTimeInSeconds - secondsPassed;

			if (secondsLeft < 0)
			{
				secondsLeft = 0;
			}

			TimeSpan timeSpan = TimeSpan.FromSeconds(secondsLeft);
			string formattedTime = timeSpan.ToString(@"hh\:mm\:ss");

			return formattedTime;
		}

		public DateTime GetDateTimeAsCEST(DateTime now)
        {
			var cest = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));

			return cest;
		}

		public IServiceOperationResult GetNewOrders(JwtDto jwtDto)
        {
			

				IServiceOperationResult operationResult;

				ISalesman salesman = (ISalesman)_helper.FindUserByJwt(jwtDto.Token);

				if (salesman == null)
				{
					operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller doesn't exist!");

					return operationResult;
				}

				List<IOrder> orders = workingRepo.OrderRepository.ItemsAndArticles(
					order => order.Items.FirstOrDefault(item => item.Article.SalesmanId == salesman.Id) != null).ToList<IOrder>();

				orders.RemoveAll(order => !IsOrderPending(order));

				orders.ForEach(order => order.Items = order.Items.ToList().FindAll(item => item.Article.SalesmanId == salesman.Id));

			OrderListDto orderListDto = new OrderListDto();
			orderListDto.Orders = new List<OrderInfoDto>();
				/*{
					Orders = _mapper.Map<List<OrderInfoDto>>(orders)
				};*/


			foreach (var o in orders)
			{
				OrderInfoDto oi = new OrderInfoDto();
				oi.Address = o.Address;
				oi.Comment = o.Comment;
				oi.Created = o.Created;
				oi.Id = o.Id;
				oi.TotalPrice = o.TotalPrice;
				oi.Items = new List<ItemDto>();
				foreach (var i in o.Items)
				{
					ItemDto io = new ItemDto();
					io.ArticleId = i.ArticleId;
					io.ArticleName = i.ArticleName;
					io.PricePerUnit = i.PricePerUnit;
					io.Quantity = i.Quantity;
					byte[] image = _helper.GetArticleProductImage(i.Article);
					io.ArticleImage = image;
					oi.Items.Add(io);
				}
				orderListDto.Orders.Add(oi);
			}


			foreach (var orderDto in orderListDto.Orders)
				{
					IOrder relatedOrder = orders.Find(x => x.Id == orderDto.Id);
					orderDto.RemainingTime = CalculateDeliveryRemainingTime(orderDto.Created, relatedOrder.DeliveryInSeconds);
				}

				operationResult = new ServiceOperationResult(true, orderListDto);

				return operationResult;


			
		}

		public IServiceOperationResult GetOrderInfo(JwtDto jwtDto, long id) {

			IServiceOperationResult operationResult;

			ISalesman salesman = (Salesman)_helper.FindUserByJwt(jwtDto.Token);

			if (salesman == null) { 
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Prodavac ne postoji!");
				return operationResult;
			}

			IOrder order = workingRepo.OrderRepository.GetById(id);

			if (order == null) { 
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Porudzbina ne postoji!");
				return operationResult;

			}

			OrderInfoDto orderDto = _mapper.Map<OrderInfoDto>(order);
			orderDto.RemainingTime = CalculateDeliveryRemainingTime(orderDto.Created, order.DeliveryInSeconds);

			List<IItem> items = workingRepo.ItemRepository.FindAllIncludeArticles((item) => item.OrderId == id).ToList<IItem>();
			items.RemoveAll(item => item.Article.SalesmanId != salesman.Id);
			//orderDto.Items = _mapper.Map<List<ItemDto>>(items);
			orderDto.Items = new List<ItemDto>();

				foreach (var i in items)
				{
					ItemDto io = new ItemDto();
					io.ArticleId = i.ArticleId;
					io.ArticleName = i.ArticleName;
					io.PricePerUnit = i.PricePerUnit;
					io.Quantity = i.Quantity;
					byte[] image = _helper.GetArticleProductImage(i.Article);
					io.ArticleImage = image;
					orderDto.Items.Add(io);
				}


			operationResult = new ServiceOperationResult(true, orderDto);

			return operationResult;

		}

		public IServiceOperationResult UpdateArticleProductImage(UpdateArticleImageDto articleDto, JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			ISalesman seller = (ISalesman)_helper.FindUserByJwt(jwtDto.Token);
			if (seller == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Salesman doesn't exist!");

				return operationResult;
			}

			if (((Salesman)seller).ApprovalStatus != Status.APPROVED)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller isn't approved!");

				return operationResult;
			}

			IArticle article = (workingRepo.ArticleRepository.FindFirst(x => x.SalesmanId == seller.Id && x.Name == articleDto.Name));
			if (article == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound,
					$"Article named \"{articleDto.Name}\" doesn't exist among sellers aricles!");

				return operationResult;
			}

			if (articleDto.Image == null)
			{
				_helper.DeleteArticleProductImageIfExists(article);
				article.Image = null;
			}

			_helper.AddProductImageIfExists(article, articleDto.Image, seller.Id);

			workingRepo.ArticleRepository.Update((Article)article);
			workingRepo.Commit();

			operationResult = new ServiceOperationResult(true);

			return operationResult;
		}

	}
}
