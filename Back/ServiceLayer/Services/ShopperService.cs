using AutoMapper;
using DataLayer;
using DataLayer.Models;
using DataLayer.Models.Interfaces;
using ServiceLayer.DataBase.ArticleDto;
using ServiceLayer.DataBase.Auth;
using ServiceLayer.DataBase.Item;
using ServiceLayer.DataBase.Order;
using ServiceLayer.Helpers;
using ServiceLayer.Services.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class ShopperService : IShopperService
    {
        private readonly IWorkingRepository workingRepo;
        private readonly IMapper mapper;
        private readonly IHelper helper;

		public ShopperService(IWorkingRepository workingRepo, IMapper mapper, IHelper helper)
		{
			this.workingRepo = workingRepo;
			this.mapper = mapper;
			this.helper = helper;
		}

		public IServiceOperationResult GetAllArticles()
        {
            IServiceOperationResult operationResult;

            List<IArticle> articles = workingRepo.ArticleRepository.GetAll().ToList<IArticle>();
            List<ArticleDetailDto> temp = helper.ReturnArticlesDetail(articles);
            ArticleListDto dtoList = new ArticleListDto() {Articles = temp};

            operationResult = new ServiceOperationResult(true, dtoList);

            return operationResult;
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

        public IServiceOperationResult GetOrderInfo(long id) {

            IServiceOperationResult operationResult;

            IOrder order = workingRepo.OrderRepository.GetById(id);

            if (order == null)
            {
                operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Porudzbina ne postoji!");
                return operationResult;

            }

            OrderInfoDto orderDto = mapper.Map<OrderInfoDto>(order);
            orderDto.RemainingTime = CalculateDeliveryRemainingTime(orderDto.Created, order.DeliveryInSeconds);

            List<IItem> items = workingRepo.ItemRepository.FindAllIncludeArticles((item) => item.OrderId == id).ToList<IItem>();
			//orderDto.Items = mapper.Map<List<ItemDto>>(items);
			orderDto.Items = new List<ItemDto>();

			foreach (var i in items)
			{
				ItemDto io = new ItemDto();
				io.ArticleId = i.ArticleId;
				io.ArticleName = i.ArticleName;
				io.PricePerUnit = i.PricePerUnit;
				io.Quantity = i.Quantity;
				byte[] image = helper.GetArticleProductImage(i.Article);
				io.ArticleImage = image;
				orderDto.Items.Add(io);
			}

			foreach (var orderItem in orderDto.Items)
			{
				IArticle article = items.Find(item => item.ArticleId == orderItem.ArticleId).Article;
				byte[] image = helper.GetArticleProductImage(article);
				orderItem.ArticleImage = image;
			}

			operationResult = new ServiceOperationResult(true, orderDto);

            return operationResult;
        }
		public IServiceOperationResult GetFinishedOrders(JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			IShopper customer = (IShopper)helper.FindUserByJwt(jwtDto.Token);
			if (customer == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Customer doesn't exist!");

				return operationResult;
			}

			List<IOrder> orders = workingRepo.OrderRepository.FindAllIncludeItems(x => x.ShopperId == customer.Id).ToList<IOrder>();

			orders = helper.GetFinishedOrders(orders);
			OrderListDto orderListDto = new OrderListDto()
			{
				Orders = mapper.Map<List<OrderInfoDto>>(orders)
			};

			foreach (var orderDto in orderListDto.Orders)
			{
				IOrder relatedOrder = orders.Find(x => x.Id == orderDto.Id);
				orderDto.RemainingTime = CalculateDeliveryRemainingTime(orderDto.Created, relatedOrder.DeliveryInSeconds);
			}

			operationResult = new ServiceOperationResult(true, orderListDto);

			return operationResult;
		}

		public IServiceOperationResult GetPendingOrders(JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			IShopper customer = (IShopper)helper.FindUserByJwt(jwtDto.Token);
			if (customer == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Customer doesn't exist!");

				return operationResult;
			}

			List<IOrder> orders = workingRepo.OrderRepository.FindAllIncludeItems(x => x.ShopperId == customer.Id).ToList<IOrder>();

			orders = helper.GetPendingOrders(orders);
			OrderListDto orderListDto = new OrderListDto()
			{
				Orders = mapper.Map<List<OrderInfoDto>>(orders)
			};

			foreach (var orderDto in orderListDto.Orders)
			{
				IOrder relatedOrder = orders.Find(x => x.Id == orderDto.Id);
				orderDto.RemainingTime = CalculateDeliveryRemainingTime(orderDto.Created, relatedOrder.DeliveryInSeconds);
			}

			operationResult = new ServiceOperationResult(true, orderListDto);

			return operationResult;
		}

		public IServiceOperationResult PlaceOrder(PlacedOrderDto orderDto, JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			IShopper customer = (IShopper)helper.FindUserByJwt(jwtDto.Token);
			if (customer == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Customer doesn't exist!");

				return operationResult;
			}

			if (orderDto.Address.Trim().Length < 5)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest, "Address is invalid!");

				return operationResult;
			}

			List<IArticle> associatedArticles = new List<IArticle>();
			foreach (var item in orderDto.Items)
			{
				if (AreStringPropsNullOrEmpty(item))
				{
					operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest, "Item data is not properly filled in!");

					return operationResult;
				}

				IArticle article = workingRepo.ArticleRepository.FindFirst(x => x.Id == item.ArticleId);

				if (article == null)
				{
					operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest, $"Article with id \"{item.ArticleId}\" doesn't exist!");

					return operationResult;
				}

				if (item.Quantity <= 0)
				{
					operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest, $"Quantity has to be at least 1 for every item!");

					return operationResult;
				}

				if (item.Quantity > article.Quantity)
				{
					operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest,
						$"There is not {item.Quantity} units of \"{article.Name}\" in storage!");

					return operationResult;
				}

				associatedArticles.Add(article);
			}

			Order order = mapper.Map<Order>(orderDto);

			order.TotalPrice = order.Items.Sum(item => associatedArticles.Find(article => article.Id == item.ArticleId).Price * item.Quantity);

			foreach (var item in order.Items)
			{
				IArticle article = associatedArticles.Find(article => article.Id == item.ArticleId);
				item.PricePerUnit = article.Price;
				item.ArticleName = article.Name;
				article.Quantity -= item.Quantity;
				workingRepo.ArticleRepository.Update((Article)article);
				workingRepo.ItemRepository.Add(item);
			}

			order.Created = GetDateTimeAsCEST(DateTime.Now);
			order.DeliveryInSeconds = new Random().Next(3600, 7200);
			order.ShopperId = customer.Id;

			workingRepo.OrderRepository.Add(order);

			workingRepo.Commit();

			operationResult = new ServiceOperationResult(true);

			return operationResult;
		}
		public bool AreStringPropsNullOrEmpty(object o)
		{
			bool isNullOrEmpty = o.GetType().GetProperties()
				.Where(pi => pi.PropertyType == typeof(string))
				.Select(pi => (string)pi.GetValue(o))
				.Any(value => string.IsNullOrEmpty(value));

			return isNullOrEmpty;
		}
		public IServiceOperationResult CancelOrder(long orderId, JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			IShopper customer = (IShopper)helper.FindUserByJwt(jwtDto.Token);
			if (customer == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Customer doesn't exist!");

				return operationResult;
			}

			IOrder order = workingRepo.OrderRepository.FindFirstIncludeItems(x => x.Id == orderId);
			if (order == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Order doesn't exist!");

				return operationResult;
			}

			foreach (var item in order.Items)
			{
				IArticle article = workingRepo.ArticleRepository.FindFirst(article => article.Id == item.ArticleId);
				article.Quantity += item.Quantity;
			}

			if (!IsOrderCancelable(order))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.Conflict,
					"Order can not be cancelled since it was placed more than an hour ago!");

				return operationResult;
			}

			workingRepo.OrderRepository.Remove((Order)order);
			workingRepo.Commit();

			operationResult = new ServiceOperationResult(true);

			return operationResult;
		}
		public bool IsOrderCancelable(IOrder order)
		{
			int secondsPassed = (int)(GetDateTimeAsCEST(DateTime.Now) - order.Created).TotalSeconds;
			if (secondsPassed > 3600)
			{
				return false;
			}

			return true;
		}
	}
}
