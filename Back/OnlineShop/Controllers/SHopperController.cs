using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using ServiceLayer.DataBase.Auth;
using ServiceLayer.DataBase.Order;
using ServiceLayer.Services.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Controllers
{
    [Route("api/shopper")]
    [ApiController]
       public class SHopperController : ControllerBase
    {
        IShopperService shopperService;

        public SHopperController(IShopperService shopperService)
        {
            this.shopperService = shopperService;
        }


		[HttpGet("articles")]
		[Authorize(Roles = "Shopper")]
		public IActionResult GetAllArticles()
		{
			try
			{
				IServiceOperationResult operationResult = shopperService.GetAllArticles();

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

		[HttpGet("finished-orders")]
		[Authorize(Roles = "Shopper")]
		public IActionResult GetFinishedOrders()
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult operationResult = shopperService.GetFinishedOrders(jwtDto);

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

		[HttpGet("pending-orders")]
		[Authorize(Roles = "Shopper")]
		public IActionResult GetPendingOrders()
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult operationResult = shopperService.GetPendingOrders(jwtDto);

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

		[HttpPost("order")]
		[Authorize(Roles = "Shopper")]
		public IActionResult PlaceOrder(PlacedOrderDto orderDto)
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult operationResult = shopperService.PlaceOrder(orderDto, jwtDto);

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

		[HttpDelete("order")]
		[Authorize(Roles = "Shopper")]
		public IActionResult CancelOrder([FromQuery] long orderId)
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult operationResult = shopperService.CancelOrder(orderId, jwtDto);

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

		[HttpGet("order")]
		[Authorize(Roles = "Shopper")]
		public IActionResult GetOrderDetails([FromQuery] long id)
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult operationResult = shopperService.GetOrderInfo(id);

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
	}
}
