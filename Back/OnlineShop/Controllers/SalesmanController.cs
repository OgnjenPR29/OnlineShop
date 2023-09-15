using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using ServiceLayer.DataBase.ArticleDto;
using ServiceLayer.DataBase.Auth;
using ServiceLayer.Services.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Controllers
{
    [Route("api/salesman")]
    [ApiController]
    public class SalesmanController : Controller
    {
        ISalesmanService salesmanService;

        public SalesmanController(ISalesmanService sellerService)
        {
            salesmanService = sellerService;
        }

        [HttpPost("article")]
        [Authorize(Roles = "Salesman")]
        public IActionResult AddArticle([FromForm] NewArticleDto article)
        {
            try 
            {
                string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
                JwtDto jwtDto = new JwtDto(token);

                IServiceOperationResult result = salesmanService.AddArticle(article, jwtDto);

                if (!result.IsSuccessful)
                {
                    return StatusCode((int)result.ErrorCode, result.ErrorMessage);
                }

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);

            }
        }

        [HttpGet("articles")]
        [Authorize(Roles = "Salesman")]
        public IActionResult GetArticles()
        {
            try
            {
                string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
                JwtDto jwtDto = new JwtDto(token);

                IServiceOperationResult operationResult = salesmanService.GetAllArticles(jwtDto);

                if (!operationResult.IsSuccessful)
                {
                    return StatusCode((int)operationResult.ErrorCode, operationResult.ErrorMessage);
                }

                return Ok(operationResult.Dto);

            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut("article")]
        [Authorize(Roles = "Salesman")]
        public IActionResult UpdateArticle([FromBody] UpdateArticleDto article)
        {
            try
            {
                string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
                JwtDto jwtDto = new JwtDto(token);

                IServiceOperationResult result = salesmanService.UpdateArticle(article, jwtDto);

                if (!result.IsSuccessful)
                {
                    return StatusCode((int)result.ErrorCode, result.ErrorMessage);
                }

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("article")]
        [Authorize(Roles = "Salesman")]
        public IActionResult DeleteArticle([FromQuery] string name)
        {
            try
            {
                string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
                JwtDto jwtDto = new JwtDto(token);

                IServiceOperationResult result = salesmanService.DeleteArticle(name, jwtDto);

                if (!result.IsSuccessful)
                {
                    return StatusCode((int)result.ErrorCode, result.ErrorMessage);
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("article")]
        [Authorize(Roles = "Salesman")]
        public IActionResult GetArticleInfo([FromQuery]string name)
        {
            try
            {
                string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
                JwtDto jwtDto = new JwtDto(token);

                IServiceOperationResult operationResult = salesmanService.GetArticleInfo(name,jwtDto);

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


        [HttpGet("previous-orders")]
        [Authorize(Roles = "Salesman")]
        public IActionResult PreviousOrders()
        {
            try
            {
                string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
                JwtDto jwtDto = new JwtDto(token);

                IServiceOperationResult operationResult = salesmanService.GetPreviousOrders(jwtDto);

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

        [HttpGet("new-orders")]
        [Authorize(Roles = "Salesman")]
        public IActionResult NewOrders()
        {
            try
            {
                string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
                JwtDto jwtDto = new JwtDto(token);

                IServiceOperationResult operationResult = salesmanService.GetNewOrders(jwtDto);

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

        [HttpGet("order")]
        [Authorize(Roles = "Salesman")]
        public IActionResult GetOrderInfo([FromQuery] long id)
        {
            try
            {
                string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
                JwtDto jwtDto = new JwtDto(token);

                IServiceOperationResult operationResult = salesmanService.GetOrderInfo(jwtDto, id);

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

        [HttpPut("product-image")]
        [Authorize(Roles = "Salesman")]
        public IActionResult UpdateArticleProductImage([FromForm] UpdateArticleImageDto article)
        {
            try
            {
                string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
                JwtDto jwtDto = new JwtDto(token);

                IServiceOperationResult result = salesmanService.UpdateArticleProductImage(article, jwtDto);

                if (!result.IsSuccessful)
                {
                    return StatusCode((int)result.ErrorCode, result.ErrorMessage);
                }

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
