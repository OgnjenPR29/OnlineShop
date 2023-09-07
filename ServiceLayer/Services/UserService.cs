using AutoMapper;
using DataLayer;
using DataLayer.Models;
using DataLayer.Models.Interfaces;
using ServiceLayer.DataBase.Auth;
using ServiceLayer.DataBase.User;
using ServiceLayer.Helpers;
using ServiceLayer.Services.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class UserService : IUserService
    {


		IWorkingRepository workingRepo;

		IMapper _mapper;

		IHelper _helper;

		public UserService(IWorkingRepository workingRepo, IMapper mapper, IHelper helper)
		{
			this.workingRepo = workingRepo;
			_mapper = mapper;
			_helper = helper;
		}


		public IServiceOperationResult GetUser(JwtDto jwtDto)
        {
			IServiceOperationResult operationResult;

			IUser user = _helper.FindUserByJwt(jwtDto.Token);
			if (user == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound);

				return operationResult;
			}

			UserDto userDto = _mapper.Map<UserDto>(user);
			operationResult = new ServiceOperationResult(true, userDto);

			return operationResult;
		}

		public IServiceOperationResult UpdateUser(BasicInfoDto newUserDto, JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			IUser newUser = _mapper.Map<User>(newUserDto);

			IUser currentUser = _helper.FindUserByJwt(jwtDto.Token);
			if (currentUser == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound);

				return operationResult;
			}

			if (currentUser == null || newUser == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound);

				return operationResult;
			}

			if (newUser.Username != currentUser.Username && _helper.FindUserByUsername(newUser.Username) != null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.Conflict, "A user with a given username already exists!");

				return operationResult;
			}

			//_helper.UpdateProfileImagePath(currentUser, newUser.Username);
			_helper.UpdateBasicUserData(currentUser, newUser);

			if (!UpdateUser(currentUser))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound);

				return operationResult;
			}

			workingRepo.Commit();

			operationResult = new ServiceOperationResult(true);

			return operationResult;
		}

		public bool UpdateUser(IUser user)
		{
			if (user is Admin)
			{
				workingRepo.AdminRepository.Update((Admin)user);
			}
			else if (user is Shopper)
			{
				workingRepo.ShopperRepository.Update((Shopper)user);
			}
			else if (user is Salesman)
			{
				workingRepo.SalesmanRepository.Update((Salesman)user);
			}
			else
			{
				return false;
			}

			return true;
		}

		public IServiceOperationResult ChangePassword(PassChangeDto passwordDto, JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			IUser user = _helper.FindUserByJwt(jwtDto.Token);
			if (user == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound);

				return operationResult;
			}

			if (!_helper.IsPassOK(passwordDto.OldPass, user.Password))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.Unauthorized, "Invalid current password!");

				return operationResult;
			}

			if (_helper.IsPassWeak(passwordDto.NewPass))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest, "Password should be at least 6 characters long!");

				return operationResult;
			}

			string newHashedPass = BCrypt.Net.BCrypt.HashPassword(passwordDto.NewPass);
			user.Password = newHashedPass;

			if (!UpdateUser(user))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest);

				return operationResult;
			}

			workingRepo.Commit();

			operationResult = new ServiceOperationResult(true);

			return operationResult;
		}
	}
}
