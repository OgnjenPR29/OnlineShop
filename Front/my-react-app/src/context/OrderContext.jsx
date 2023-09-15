import { createContext, useCallback, useEffect, useState } from 'react';

const OrderContext = createContext({
  addItemToOrder: (article, quantity) => {},
  removeOrder: () => {},
  updateOrder: (newOrder) => {},
  hasArticleWithId: (articleId) => {},
  hasItems: () => {},
  getOrderDto: () => {},
  order: {},
  items: {},
});

export const OrderContextProvider = ({ children }) => {
  const [order, setOrder] = useState(orderInit);

  const addToLocalStorage = useCallback(() => {
    window.localStorage.setItem('order', JSON.stringify(order));
  }, [order]);

  const removeFromLocalStorage = useCallback(() => {
    window.localStorage.removeItem('order');
  }, []);

  useEffect(() => {
    addToLocalStorage();
  }, [addToLocalStorage]);

  const addItemToOrder = useCallback((article, quantity) => {
    setOrder((prevOrder) => {
      const updatedOrder = { ...prevOrder };
      if (!updatedOrder.items[article.id]) {
        updatedOrder.items[article.id] = quantity;
        if (!updatedOrder.articles[article.id]) {
          updatedOrder.articles[article.id] = article;
        }
      } else {
        updatedOrder.items[article.id].quantity = quantity;
        if (!updatedOrder.articles[article.id]) {
          updatedOrder.articles[article.id] = article;
        }
      }
      return updatedOrder;
    });
  }, []);

  const removeOrder = useCallback(() => {
    setOrder((prevOrder) => ({
      ...prevOrder,
      items: {},
      articles: {},
      address: '',
      comment: '',
    }));

    removeFromLocalStorage();
  }, [removeFromLocalStorage]);

  const updateOrder = useCallback((newOrder) => {
    const itemsToRemove = [];

    for (var key in newOrder.items) {
      if (newOrder.items[key] < 1) {
        itemsToRemove.push(key);
      }
    }

    for (var i = 0; i < itemsToRemove.length; i++) {
      var item = itemsToRemove[i];
      delete newOrder.items[item];
      delete newOrder.articles[item];
    }

    setOrder(newOrder);
  }, []);

  const hasArticleWithId = useCallback(
    (articleId) => {
      return order.items.hasOwnProperty(articleId);
    },
    [order]
  );

  const hasItems = useCallback(() => {
    return Object.keys(order.items).length > 0;
  }, [order]);

  const getOrderDto = useCallback(() => {
    const transformedArray = Object.entries(order.items).map(
      ([key, value]) => ({
        ArticleId: key,
        Quantity: value,
      })
    );

    return {
      comment: order.comment,
      address: order.address,
      items: transformedArray,
    };
  }, [order]);

  return (
    <OrderContext.Provider
      value={{
        addItemToOrder,
        removeOrder,
        updateOrder,
        hasArticleWithId,
        hasItems,
        getOrderDto,
        order,
        items: order.items,
      }}
    >
      {children}
    </OrderContext.Provider>
  );
};

const orderInit = {
  comment: '',
  address: '',
  items: {},
  articles: {},
};

export default OrderContext;
