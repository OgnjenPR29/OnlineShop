using AutoMapper;
using DataLayer;
using DataLayer.Models.Interfaces;
using ServiceLayer.DataBase;
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
    }
}
