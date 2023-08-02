using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using ServiceLayer.DataBase.Auth;
using ServiceLayer.Services.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        IAuthService authService;
        public AuthentificationController(IAuthService userAuthService)
        {
            authService = userAuthService;
        }

		[HttpPost("login")]
		public IActionResult LoginUser([FromBody] LoginDto loginDto)
		{
			try
			{
				IServiceOperationResult operationResult = authService.LoginUser(loginDto);

				if (!operationResult.IsSuccessful)
				{
					return StatusCode((int)operationResult.ErrorCode, operationResult.ErrorMessage);
				}

				return Ok(operationResult.Dto);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		[HttpPost("register")]
		public IActionResult RegisterUser([FromBody] RegDto regDto)
		{
			try
			{
				IServiceOperationResult operationResult = authService.RegisterUser(regDto);

				if (!operationResult.IsSuccessful)
				{
					return StatusCode((int)operationResult.ErrorCode, operationResult.ErrorMessage);
				}

				return Ok(operationResult.Dto);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[HttpPost("google-login")]
		public async Task<IActionResult> GoogleLogin(string idToken)
		{
			try
			{
				GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
				authService.GoogleLogin(payload);

				return Ok();
			}
			catch (InvalidJwtException ex)
			{
				return Unauthorized(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

	}
}
