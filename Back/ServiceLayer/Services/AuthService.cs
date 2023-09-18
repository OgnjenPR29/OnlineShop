using AutoMapper;
using DataLayer;
using DataLayer.Models;
using DataLayer.Models.Interfaces;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using ServiceLayer.DataBase.Auth;
using ServiceLayer.Helpers;
using ServiceLayer.Services.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class AuthService : IAuthService
    {

        private IMapper _mapper;

        private IHelper helper;

        private IWorkingRepository workingRepository;

        public AuthService(IHelper helper, IMapper mapper, IWorkingRepository workingRepository)
        {
            this.helper = helper;
            this.workingRepository = workingRepository;
            _mapper = mapper;
        }

        public IServiceOperationResult LoginUser(LoginDto loginDto)
        {
            IServiceOperationResult operationResult;

            IUser user = helper.FindUserByUsername(loginDto.Username);
            
            if(user == null)
            {
                operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Ne postoji user sa tim username.");
                return operationResult;
            }
            
            if(helper.IsPassOK(loginDto.Password, user.Password) != true)
            {
                operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.Unauthorized, "Pogresna sifra");
                return operationResult;
            }

            string token = helper.IssueUserJwt(user);

            if (token == null)
            {
                operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.InternalServerError);

                return operationResult;
            }

            operationResult = new ServiceOperationResult(true, new JwtDto(token));

            return operationResult;

        }

        public IServiceOperationResult RegisterUser(RegDto registerDto)
        {
            IServiceOperationResult operationResult;

            if (helper.FindUserByUsername(registerDto.Username) != null || helper.FindUserByEmail(registerDto.Email) != null)
            {
                operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.Conflict, "Username ili mejl vec postoje");

                return operationResult;
            }

            if (helper.IsPassWeak(registerDto.Password))
            {
                operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest, "Sifra mora imati najmanje 7 karaktera");

                return operationResult;
            }

            try
            {
                var mailAddress = new MailAddress(registerDto.Email);
            }
            catch
            {

                operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest, "Email adresa nije validna");

                return operationResult;
            }

            registerDto.Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            ;

            IUser newUser = CreateUserAndAddToRepository(registerDto);

            if (newUser == null)
            {
                operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest);

                return operationResult;
            }

            workingRepository.Commit();

            operationResult = new ServiceOperationResult(true);

            return operationResult;

        }
        private IFormFile DownloadProfileImage(string imageUrl)
        {
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(imageUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var stream = response.Content.ReadAsStreamAsync().Result;

                    var formFile = new FormFile(stream, 0, stream.Length, "profileImage", "profile.jpg");

                    return formFile;
                }
            }

            return null;
        }

        private IUser CreateUserAndAddToRepository(RegDto registerDto)
        {
            if (registerDto.Role == UserType.Admin.ToString())
            {
                Admin admin = _mapper.Map<Admin>(registerDto);
                helper.UploadProfileImage(admin, registerDto.Image);
                workingRepository.AdminRepository.Add(admin);

                return admin;
            }
            else if (registerDto.Role == UserType.Shopper.ToString())
            {
                Shopper customer = _mapper.Map<Shopper>(registerDto);
                helper.UploadProfileImage(customer, registerDto.Image);
                workingRepository.ShopperRepository.Add(customer);

                return customer;
            }
            else if (registerDto.Role == UserType.Salesman.ToString())
            {
                Salesman seller = _mapper.Map<Salesman>(registerDto);
                seller.ApprovalStatus = Status.PENDING;
                helper.UploadProfileImage(seller, registerDto.Image);
                workingRepository.SalesmanRepository.Add(seller);

                return seller;
            }

            return null;
        }

        public IServiceOperationResult GoogleLogin(GoogleJsonWebSignature.Payload payload)
        {
            IServiceOperationResult operationResult;

            var user = new Shopper
            {
                Id = long.Parse(payload.Subject),
                Firstname = payload.GivenName,
                Lastname = payload.FamilyName,
                Username = payload.Name,
                Email = payload.Email,
            };

            Shopper shopper = user;

            workingRepository.ShopperRepository.Add(shopper);
            workingRepository.Commit();

            operationResult = new ServiceOperationResult(true);

            return operationResult;
        }

    }
}
