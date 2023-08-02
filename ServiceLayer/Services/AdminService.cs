using AutoMapper;
using DataLayer;
using DataLayer.Models;
using DataLayer.Models.Interfaces;
using ServiceLayer.DataBase;
using ServiceLayer.DataBase.ArticleDto;
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

            operationResult = new ServiceOperationResult(true, dto);

            return operationResult;


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


    }
}
