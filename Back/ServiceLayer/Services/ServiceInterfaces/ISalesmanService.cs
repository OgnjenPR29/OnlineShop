using ServiceLayer.DataBase.ArticleDto;
using ServiceLayer.DataBase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.ServiceInterfaces
{
    public interface ISalesmanService
    {
        IServiceOperationResult AddArticle(NewArticleDto articleDto, JwtDto jwtDto);
        IServiceOperationResult GetAllArticles(JwtDto jwtDto);
        IServiceOperationResult UpdateArticle(UpdateArticleDto articleDto, JwtDto jwtDto);
        IServiceOperationResult DeleteArticle(string articleName, JwtDto jwtDto);
        IServiceOperationResult GetArticleInfo(string articleName, JwtDto jwtDto);
        IServiceOperationResult GetPreviousOrders(JwtDto jwtDto);
        IServiceOperationResult GetNewOrders(JwtDto jwtDto);
        IServiceOperationResult GetOrderInfo(JwtDto jwtDto, long id);
        IServiceOperationResult UpdateArticleProductImage(UpdateArticleImageDto articleDto, JwtDto jwtDto);




        }
}
