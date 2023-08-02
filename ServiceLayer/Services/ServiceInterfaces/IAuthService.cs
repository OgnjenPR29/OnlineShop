using Google.Apis.Auth;
using ServiceLayer.DataBase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.ServiceInterfaces
{
    public interface IAuthService
    {
        IServiceOperationResult LoginUser(LoginDto loginDto);

        IServiceOperationResult RegisterUser(RegDto registerDto);

        IServiceOperationResult GoogleLogin(GoogleJsonWebSignature.Payload payload);
    }
}
