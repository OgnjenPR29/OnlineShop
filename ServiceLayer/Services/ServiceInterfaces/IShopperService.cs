using ServiceLayer.DataBase.Auth;
using ServiceLayer.DataBase.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.ServiceInterfaces
{
    public interface IShopperService
	{
		IServiceOperationResult GetAllArticles();

		IServiceOperationResult GetPendingOrders(JwtDto jwtDto);

		IServiceOperationResult GetFinishedOrders(JwtDto jwtDto);

		IServiceOperationResult PlaceOrder(PlacedOrderDto orderDto, JwtDto jwDto);

		IServiceOperationResult CancelOrder(long orderId, JwtDto jwtDto);

		IServiceOperationResult GetOrderInfo(long id);
	}
}
