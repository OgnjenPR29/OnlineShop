using AutoMapper;
using DataLayer;
using DataLayer.Models;
using DataLayer.Models.Interfaces;
using ServiceLayer.DataBase;
using ServiceLayer.DataBase.ArticleDto;
using ServiceLayer.DataBase.Item;
using ServiceLayer.DataBase.Salesman;
using ServiceLayer.Helpers;
using ServiceLayer.Services.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class AdminService : IAdminService
    {
        IWorkingRepository workingRepo;
        IMapper _mapper;
        IHelper helper;
            
        public AdminService(IWorkingRepository workingRepository, IMapper mapper, IHelper helper)
        {
            workingRepo = workingRepository;
            _mapper = mapper;
            this.helper = helper;
        }

        public IServiceOperationResult AllSalesmans()
        {
            IServiceOperationResult operationResult;

            List<ISalesman> salesmans = workingRepo.SalesmanRepository.GetAll().ToList<ISalesman>();

            List<SalesmanDto> temp = _mapper.Map<List<SalesmanDto>>(salesmans);

            SalesmanListDto dto = new SalesmanListDto() { Salesmans = temp };

            operationResult = new ServiceOperationResult(true, dto);

            return operationResult;


        }
        
        public IServiceOperationResult AllOrders()
        {
            IServiceOperationResult operationResult;

            List<IOrder> orders = workingRepo.OrderRepository.GetAll().ToList<IOrder>();

            List<OrderInfoDto> temp = _mapper.Map<List<OrderInfoDto>>(orders);

            OrderListDto dto = new OrderListDto() { Orders = temp };

            foreach (var orderDto in dto.Orders)
            {
                IOrder relatedOrder = orders.Find(x => x.Id == orderDto.Id);
                orderDto.RemainingTime = CalculateDeliveryRemainingTime(orderDto.Created, relatedOrder.DeliveryInSeconds);
            }

            operationResult = new ServiceOperationResult(true, dto);

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

        public IServiceOperationResult ChangeSalesmanStatus(ApprovalStatusDto status)
        {
            IServiceOperationResult operationResult;

            ISalesman salesman = (ISalesman)helper.FindUserByUsername(status.SalesmanName);

            if (salesman == null)
            {
                operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Prodavac ne postoji.");

                return operationResult;
            }

            salesman.ApprovalStatus = status.SalesmanStatus? Status.APPROVED : Status.DENIED;

            workingRepo.SalesmanRepository.Update((Salesman)salesman);
            workingRepo.Commit();

            operationResult = new ServiceOperationResult(true);

            return operationResult;

        }

        public IServiceOperationResult GetOrderDetails(long id)
        {
            IServiceOperationResult operationResult;

            IOrder order = workingRepo.OrderRepository.GetById(id);

            if (order == null)
            {
                operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound,
                    $"Order with the id \"{id}\" has not been found!");

                return operationResult;
            }

            OrderInfoDto orderDto = _mapper.Map<OrderInfoDto>(order);
            orderDto.RemainingTime = CalculateDeliveryRemainingTime(orderDto.Created, order.DeliveryInSeconds);

            List<IItem> items = workingRepo.ItemRepository.FindAllIncludeArticles((item) => item.OrderId == id).ToList<IItem>();
            //orderDto.Items = _mapper.Map<List<ItemDto>>(items);

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


    }
}
