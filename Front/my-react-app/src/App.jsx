import { Routes, Route, Router } from 'react-router-dom';
import { useContext, useEffect } from 'react';
import UserContext from './context/UserContext';
import Navbar from './components/Navbar';

import { Box } from '@mui/material';

import Home from './page/Home';
import Login from './page/Authentication/Login';
import Register from './page/Authentication/Register';
import NotFound from './page/NotFound';
import Profile from './page/Common/Profile';


function App() {
  const { loadUser, ...userContext } = useContext(UserContext);

  const isLoggedin = userContext.isLoggedin;
  
  const role = isLoggedin && userContext.role.toLowerCase();
  
  const approvedSeller =
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
        <Route path='*' element={<NotFound />} />
      </Routes>
    </Box>
  );
}

export default App;
