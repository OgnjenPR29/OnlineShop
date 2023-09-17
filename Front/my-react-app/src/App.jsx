import { Routes, Route, Router } from 'react-router-dom';
import { useContext, useEffect } from 'react';
import { Box } from '@mui/material';


import UserContext from './context/UserContext';
import OrderContext from './context/OrderContext';
import OrderDetails from './components/Orders/OrderDetails';

import Navbar from './components/Navbar';

import Home from './page/Home';
import Login from './page/Authentication/Login';
import Register from './page/Authentication/Register';
import NotFound from './page/NotFound';
import Profile from './page/Common/Profile';

import AllSalesmans from './page/Admin/AllSalesmans';
import AllOrders from './page/Admin/AllOrders';

import SalesmansArticles from './page/salesman/SalesmansArticles';

import SalesmansFinishedOrders from './page/salesman/SalesmansFinishedOrders';
import SalesmansPendingOrders from './page/salesman/SalesmansPendingOrders';
import ArticleDetails from './components/Articles/ArticleDetails';
import NewArticle from './page/salesman/NewArticle';

import ShopperPendingOrders from './page/ShopperPendingOrders';
import ShopperFinishedOrders from './page/ShopperFinishedOrders';
import ShopperArticles from './page/ShopperArticles';
import Order from './page/Order';

function App() {
  const { loadUser, ...userContext } = useContext(UserContext);
  const { removeOrder, ...orderContext } = useContext(OrderContext);


  const isLoggedin = userContext.isLoggedin;
  
  const role = isLoggedin && userContext.role.toLowerCase();
  
  const approvedSalesman =
    role === 'salesman' && userContext.status?.toLowerCase() === 'approved';

  useEffect(() => {
    loadUser();
  }, [loadUser]);


  useEffect(() => {
    if (!isLoggedin || role !== 'shopper') {
      removeOrder();
    }
  }, [isLoggedin, removeOrder, role]);

  console.log("usaoo")

  return (
    <Box sx={{ width: '100%', height: '100%' }}>
      <Navbar />
      <Routes>
        <Route path='/' element={<Home />} />
        {!isLoggedin && <Route path='/login' element={<Login />} />}
        {isLoggedin && <Route path='/login' element={<Home />} />}
        {!isLoggedin && <Route path='/register' element={<Register />} />}
        {isLoggedin && <Route path='/register' element={<Home />} />}
        {isLoggedin && <Route path='/profile' element={<Profile />} />}
        {role === 'admin' && (<Route path='/salesmans' element={<AllSalesmans />} />)}
        {role === 'admin' && <Route path='/orders' element={<AllOrders />} />}
        {role === 'admin' && (<Route path='/orders/:id' element={<OrderDetails />} />)}
        {approvedSalesman && (<Route path='/articles' element={<SalesmansArticles />} />)}
        {approvedSalesman && (<Route path='/finished-orders' element={<SalesmansFinishedOrders />} />)}
        {approvedSalesman && (<Route path='/finished-orders/:id' element={<OrderDetails />} />)}
        {approvedSalesman && (<Route path='/pending-orders' element={<SalesmansPendingOrders />} />)}
        {approvedSalesman && (<Route path='/pending-orders/:id' element={<OrderDetails />} />)}
        {approvedSalesman && (<Route path='/articles/:name' element={<ArticleDetails />} />)}
        {approvedSalesman && (<Route path='/new-article' element={<NewArticle />} />)}
        
        {role === 'shopper' && (<Route path='/articles' element={<ShopperArticles />} />)}
        {role === 'shopper' && (<Route path='/finished-orders' element={<ShopperFinishedOrders />} />)}
        {role === 'shopper' && (<Route path='/finished-orders/:id' element={<OrderDetails />} />)}
        {role === 'shopper' && (<Route path='/pending-orders' element={<ShopperPendingOrders />} />)}
        {role === 'shopper' && (<Route path='/pending-orders/:id' element={<OrderDetails />} />)}
        {role === 'shopper' && orderContext.hasItems() && (<Route path='/order' element={<Order />} />)}
        
        <Route path='*' element={<NotFound />} />
      </Routes>
    </Box>
  );
}

export default App;
