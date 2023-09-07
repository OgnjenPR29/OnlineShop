using DataLayer;
using DataLayer.Models.Interfaces;
using Microsoft.Extensions.Configuration;
using ServiceLayer.DataBase.ArticleDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Helpers
{
    public interface IHelper
    {

        //WINDOWS-GED5GA0\SQLEXPRESS
        public IUser FindUserByUsername(string username);

        public IUser FindUserByEmail(string email);

        public bool IsPassOK(string pass, string passCheck);

        public bool IsPassWeak(string password);

        public string IssueJwt(IEnumerable<Claim> claims);

        public string IssueUserJwt(IUser user);

        public IUser FindUserByJwt(string token);
        public List<ArticleDetailDto> ReturnArticlesDetail(List<IArticle> articles);
        public void UpdateBasicUserData(IUser currentUser, IUser newUser);
        public bool IsOrderPending(IOrder order);
        public List<IOrder> GetFinishedOrders(List<IOrder> orders);
        public List<IOrder> GetPendingOrders(List<IOrder> orders);



    }
}