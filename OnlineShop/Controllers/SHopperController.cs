using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
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
		[Authorize(Roles = "Customer")]
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
	}
}
