using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceLayer.Services.ServiceInterfaces;
using ServiceLayer.Services;
using Microsoft.AspNetCore.Authorization;
using ServiceLayer;
using Microsoft.AspNetCore.Http;

namespace OnlineShop.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : Controller
    {
		readonly IAdminService _adminService;

		public AdminController(IAdminService adminService)
        {
			_adminService = adminService;
        }

		[HttpGet("salesmans")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetSalesmans()
        {
			try
			{
				IServiceOperationResult operationResult = _adminService.AllSalesmans();

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
