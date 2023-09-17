import { useEffect, useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';

import Orders from '../components/Orders/Orders';
import useServices from '../services/useServices';
import UserContext from '../context/UserContext';

const ShopperPendingOrders = () => {
  const {
    data,
    error,
    statusCode,
    isLoading,
    getShopperPendingOrdersRequest,
    deleteShopperOrderRequest,
    clearRequest,
  } = useServices();
  const [orders, setOrders] = useState([]);
  const [deletingOrder, setDeletingOrder] = useState(false);
  const { role } = useContext(UserContext);
  const navigate = useNavigate();

  useEffect(() => {
    getShopperPendingOrdersRequest();
  }, [getShopperPendingOrdersRequest]);

  useEffect(() => {
    if (isLoading) {
      return;
    } else if (statusCode === 200 && !error && data) {
      setOrders(data.orders);
      clearRequest();
    } else if (statusCode === 200 && !error && deletingOrder) {
      alert('Order canceled!');
      setDeletingOrder(false);
      getShopperPendingOrdersRequest();
      clearRequest();
    } else if (statusCode !== 200 && error) {
      alert(statusCode, error);
    }
  }, [
    isLoading,
    statusCode,
    error,
    data,
    clearRequest,
    getShopperPendingOrdersRequest,
    deletingOrder,
  ]);

  const handleButton = (id) => {
    navigate('/pending-orders/' + id);
  };

  const handleOrderCancel = (orderId) => {
    setDeletingOrder(true);
    deleteShopperOrderRequest(orderId);
  };

  return (
    <>
      {!isLoading && (
        <Orders
          data={orders}
          role={role}
          hasButton={true}
          buttonCallback={handleButton}
          buttonText='Details'
          cancelOrderCallback={handleOrderCancel}
        ></Orders>
      )}
    </>
  );
};

export default ShopperPendingOrders;
