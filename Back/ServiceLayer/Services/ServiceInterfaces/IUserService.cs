using ServiceLayer.DataBase.Auth;
using ServiceLayer.DataBase.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.ServiceInterfaces
{
    public interface IUserService
    {
        public IServiceOperationResult GetUser(JwtDto dto);
        public IServiceOperationResult ChangePassword(PassChangeDto passwordDto, JwtDto jwtDto);

        public IServiceOperationResult UpdateUser(BasicInfoDto newUserDto, JwtDto jwtDto);


    }
}
