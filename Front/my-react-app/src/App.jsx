import { Routes, Route, Router } from 'react-router-dom';
import { useContext, useEffect } from 'react';
import UserContext from './context/UserContext';
import OrderContext from './context/OrderContext';
import OrderDetails from './components/Orders/OrderDetails';
import Navbar from './components/Navbar';

import AllSalesmans from './page/Admin/AllSalesmans';
import AllOrders from './page/Admin/AllOrders';

import { Box } from '@mui/material';

import Home from './page/Home';
import Login from './page/Authentication/Login';
import Register from './page/Authentication/Register';
import NotFound from './page/NotFound';
import Profile from './page/Common/Profile';

import SalesmansArticles from './page/salesman/SalesmansArticles';

import SalesmansFinishedOrders from './page/salesman/SalesmansFinishedOrders';
import SalesmansPendingOrders from './page/salesman/SalesmansPendingOrders';
import ArticleDetails from './components/Articles/ArticleDetails';
import NewArticle from './page/salesman/NewArticle';


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
        
        <Route path='*' element={<NotFound />} />
      </Routes>
    </Box>
  );
}

export default App;
