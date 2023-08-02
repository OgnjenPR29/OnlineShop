using AutoMapper;
using DataLayer;
using DataLayer.Models;
using DataLayer.Models.Interfaces;
using ServiceLayer.DataBase.ArticleDto;
using ServiceLayer.DataBase.Auth;
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

			/*if (((Salesman)seller).ApprovalStatus != SellerApprovalStatus.Approved)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller isn't approved!");

				return operationResult;
			}*/

			if (workingRepo.ArticleRepository.FindFirst(x => x.SalesmanId == seller.Id && x.Name == articleDto.Name) != null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, $"Prodavac ima proizvod sa imenom\"{articleDto.Name}\"!");

				return operationResult;
			}

			Article article = _mapper.Map<Article>(articleDto);
			article.SalesmanId = seller.Id;

			//sellerHelper.AddProductImageIfExists(article, articleDto.ProductImage, seller.Id);

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

			/*if (((Salesman)seller).ApprovalStatus != Status.Approved)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller isn't approved!");

				return operationResult;
			}*/

			IArticle article = workingRepo.ArticleRepository.FindFirst(x => x.Name == articleName && x.SalesmanId == seller.Id);
			if (article == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "The article doesn't exist!");

				return operationResult;
			}

			workingRepo.ArticleRepository.Remove((Article)article);
			workingRepo.Commit();

			//sellerHelper.DeleteArticleProductImageIfExists(article);

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

			/*if (((Seller)seller).ApprovalStatus != SellerApprovalStatus.Approved)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller isn't approved!");

				return operationResult;
			}*/

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

			/*if (((Salesman)seller).ApprovalStatus != SellerApprovalStatus.Approved)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller isn't approved!");

				return operationResult;
			}*/

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

			/*if (((Seller)seller).ApprovalStatus != SellerApprovalStatus.Approved)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller isn't approved!");

				return operationResult;
			}*/

			IArticle article = workingRepo.ArticleRepository.FindFirst(article => article.Name == articleName && article.SalesmanId == salesman.Id);

			if (article == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Article not found!");

				return operationResult;
			}

			ArticleDetailDto articleDto = new ArticleDetailDto(article.Id, article.Name, article.Description, article.Quantity, article.Price, null);

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

			List<IOrder> orders = workingRepo.OrderRepository.ItemsAndArticles(
				order => order.Items.FirstOrDefault(item => item.Article.SalesmanId == salesman.Id) != null).ToList<IOrder>();

			orders.RemoveAll(order => IsOrderPending(order));

			orders.ForEach(order => order.Items = order.Items.ToList().FindAll(item => item.Article.SalesmanId == salesman.Id));

			OrderListDto orderListDto = new OrderListDto()
			{
				Orders = _mapper.Map<List<OrderInfoDto>>(orders)
			};

			foreach (var orderDto in orderListDto.Orders)
			{
				IOrder relatedOrder = orders.Find(x => x.Id == orderDto.Id);
				orderDto.RemainingTime = CalculateDeliveryRemainingTime(orderDto.PlacedTime, relatedOrder.DeliveryInSeconds);
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

				OrderListDto orderListDto = new OrderListDto()
				{
					Orders = _mapper.Map<List<OrderInfoDto>>(orders)
				};

				foreach (var orderDto in orderListDto.Orders)
				{
					IOrder relatedOrder = orders.Find(x => x.Id == orderDto.Id);
					orderDto.RemainingTime = CalculateDeliveryRemainingTime(orderDto.PlacedTime, relatedOrder.DeliveryInSeconds);
				}

				operationResult = new ServiceOperationResult(true, orderListDto);

				return operationResult;


			
		}




	}
}
