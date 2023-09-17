import { useEffect, useState, useContext } from 'react';

import useServices from '../services/useServices';
import UserContext from '../context/UserContext';
import Articles from '../components/Articles/Articles';
import OrderContext from '../context/OrderContext';

const ShopperArticles = () => {
  const {
    data,
    error,
    statusCode,
    isLoading,
    getShopperArticlesRequest,
    clearRequest,
  } = useServices();
  const [articles, setArticles] = useState([]);
  const { role } = useContext(UserContext);
  const orderContext = useContext(OrderContext);

  useEffect(() => {
    getShopperArticlesRequest();
  }, [getShopperArticlesRequest]);

  useEffect(() => {
    if (isLoading) {
      return;
    } else if (statusCode === 200 && !error && data) {
      data?.articles.forEach((item) => {
        item.image = 'data:image/*;base64,' + item.image;
      });
      setArticles(data.articles);
      clearRequest();
    } else if (statusCode !== 200 && error) {
      alert(statusCode, error);
    }
  }, [isLoading, statusCode, error, data, clearRequest]);

  const handleButton = (article) => {
    console.log(article)
    orderContext.addItemToOrder(article, 1);
  };

  return (
    <>
      {!isLoading && (
        <Articles
          data={articles}
          role={role}
          hasButton={true}
          buttonCallback={handleButton}
          buttonText='Add to order'
        ></Articles>
      )}
    </>
  );
};

export default ShopperArticles;
