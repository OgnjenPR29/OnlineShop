import { createContext, useCallback, useState } from 'react';

const CartContext = createContext({
  addItemToCart: () => {},
  removeItemFromCart: () => {},
  removeOrder: () => {},
  addOrder: () => {},
  updateOrder: () => {},
  hasArticleWithId: () => {},
  hasItems: () => {},
  getOrderDto: () => {},
  order: {},
  items: {},
});

export const CartContextProvider = ({ children }) => {
  const [order, setOrder] = useState(orderInit);

  const addItemToCart = useCallback((article) => {
    setOrder((prevOrder) => {
      const updatedOrder = { ...prevOrder };
      if (!updatedOrder.items[article.id]) {
        updatedOrder.items[article.id] = {
          quantity: article.quantity,
        };
      } else {
        updatedOrder.items[article.id].quantity += article.quantity;
      }
      return updatedOrder;
    });
  }, []);

  const removeItemFromCart = useCallback((article) => {
    setOrder((prevOrder) => {
      const updatedOrder = { ...prevOrder };
      if (!updatedOrder.items[article.id]) {
        return updatedOrder;
      } else {
        updatedOrder.items[article.id].quantity -= article.quantity;
        if (updatedOrder.items[article.id].quantity <= 0) {
          delete updatedOrder.items[article.id];
        }
        return updatedOrder;
      }
    });
  }, []);

  const removeOrder = useCallback(() => {
    setOrder((prevOrder) => ({
      ...prevOrder,
      items: {},
      address: '',
      comment: '',
    }));
  }, []);

  const addOrder = useCallback((newOrder) => {
    setOrder(() => ({
      ...newOrder,
      items: {},
    }));
  }, []);

  const updateOrder = useCallback((newOrder) => {
    setOrder((prevOrder) => ({
      ...prevOrder,
      address: newOrder.address,
      comment: newOrder.comment,
    }));
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
        Quantity: value.quantity,
      })
    );
    return transformedArray;
  }, [order]);

  return (
    <CartContext.Provider
      value={{
        addItemToCart,
        removeItemFromCart,
        removeOrder,
        addOrder,
        updateOrder,
        hasArticleWithId,
        hasItems,
        getOrderDto,
        order,
        items: order.items,
      }}
    >
      {children}
    </CartContext.Provider>
  );
};

const orderInit = {
  comment: '',
  address: '',
  items: {},
};

export default CartContext;
