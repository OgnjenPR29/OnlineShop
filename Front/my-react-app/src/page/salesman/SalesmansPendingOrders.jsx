import { useEffect, useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';

import Orders from '../../components/Orders/Orders';
import useServices from '../../services/useServices';
import UserContext from '../../context/UserContext';

const SalesmansPendingOrders = () => {
  const {
    data,
    error,
    statusCode,
    isLoading,
    getSalesmansPendingOrders,
    clearRequest,
  } = useServices();
  const [orders, setOrders] = useState([]);
  const { role } = useContext(UserContext);
  const navigate = useNavigate();

  useEffect(() => {
    getSalesmansPendingOrders();
  }, [getSalesmansPendingOrders]);

  useEffect(() => {
    if (isLoading) {
      return;
    } else if (statusCode === 200 && !error && data) {
      setOrders(data.orders);
      clearRequest();
    } else if (statusCode !== 200 && error) {
      alert(statusCode, error);
    }
  }, [isLoading, statusCode, error, data, clearRequest]);

  const handleButton = (id) => {
    navigate('/pending-orders/' + id);
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
        ></Orders>
      )}
    </>
  );
};

export default SalesmansPendingOrders;
