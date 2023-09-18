using AutoMapper;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using ServiceLayer.DataBase.Auth;
using ServiceLayer.DataBase.User;
using ServiceLayer.Helpers;
using ServiceLayer.Services.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("user")]
        [Authorize]
        public IActionResult GetUser()
        {

            try
            {
                string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();

                JwtDto jwtDto = new JwtDto(token);

                IServiceOperationResult operationResult = _userService.GetUser(jwtDto);

                if (!operationResult.IsSuccessful)
                {
                    return StatusCode((int)operationResult.ErrorCode, operationResult.ErrorMessage);
                }

                return Ok(operationResult.Dto);
            }
            catch (Exception)
            {
                Console.WriteLine("neki teksttttttttttttttt");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("user")]
        [Authorize]
        public IActionResult UpdateUser([FromBody] BasicInfoDto userDto)
        {
            try
            {
                string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
                JwtDto jwtDto = new JwtDto(token);

                IServiceOperationResult operationResult = _userService.UpdateUser(userDto, jwtDto);

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

        [HttpPut("pass")]
        [Authorize]
        public IActionResult ChangePass([FromBody] PassChangeDto passwordDto)
        {
            try
            {
                string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
                JwtDto jwtDto = new JwtDto(token);

                IServiceOperationResult operationResult = _userService.ChangePassword(passwordDto, jwtDto);

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

        [HttpGet("profile-image")]
        [Authorize]
        public IActionResult GetProfileImage()
        {
            try
            {
                string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
                JwtDto jwtDto = new JwtDto(token);

                IServiceOperationResult operationResult = _userService.GetProfileImage(jwtDto);

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


        [HttpPut("profile-image")]
        [Authorize]
        public IActionResult ChangeProfileImage([FromForm] ProfileImageDto profileImageDto)
        {
            try
            {
                string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
                JwtDto jwtDto = new JwtDto(token);

                IServiceOperationResult operationResult = _userService.UploadProfileImage(profileImageDto, jwtDto);

                if (!operationResult.IsSuccessful)
                {
                    return StatusCode((int)operationResult.ErrorCode, operationResult.ErrorMessage);
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
