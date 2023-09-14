import { useContext } from 'react';
import { Link as RouterLink, useLocation } from 'react-router-dom';

import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import IconButton from '@mui/material/IconButton';
import HomeIcon from '@mui/icons-material/Home';
import ShoppingBagIcon from '@mui/icons-material/ShoppingBag';
import { Link, Box } from '@mui/material';
import { teal } from '@mui/material/colors';

import UserContext from '../context/UserContext';

const Navbar = () => {
  const userContext = useContext(UserContext);

  const location = useLocation();
  const underlineColor = teal[300];

  const isLoggedin = userContext.isLoggedin;
  const role = isLoggedin && userContext.role.toLowerCase();
  const approvedSalesman =
    role === 'salesman' && userContext.status?.toLowerCase() === 'approved';

  return (
    <Box>
      <AppBar position='static' color='default' sx={{ padding: '5px' }}>
        <Toolbar>
          <Box sx={{ flexGrow: 1 }}>
            <Link component={RouterLink} to='/'>
              <IconButton color='primary'>
                <HomeIcon sx={{ fontSize: '40px' }} />
              </IconButton>
            </Link>
          </Box>
          {(approvedSalesman || role === 'shopper') && (
            <Link
              underline={
                location.pathname.includes('pending-orders')
                  ? 'always'
                  : 'hover'
              }
              component={RouterLink}
              to='/pending-orders'
              sx={{
                fontSize: '22px',
                marginRight: 1,
                paddingLeft: '10px',
                fontWeight: 'bold',
                textDecorationColor: underlineColor,
              }}
            >
              Pending orders
            </Link>
          )}
          {(approvedSalesman || role === 'shopper') && (
            <Link
              underline={
                location.pathname.includes('finished-orders')
                  ? 'always'
                  : 'hover'
              }
              component={RouterLink}
              to='/finished-orders'
              sx={{
                fontSize: '22px',
                marginRight: 1,
                paddingLeft: '10px',
                fontWeight: 'bold',
                textDecorationColor: underlineColor,
              }}
            >
              Finished orders
            </Link>
          )}
          {(approvedSalesman || role === 'shopper') && (
            <Link
              underline={
                location.pathname.includes('articles') ? 'always' : 'hover'
              }
              component={RouterLink}
              to='/articles'
              sx={{
                fontSize: '22px',
                marginRight: 1,
                paddingLeft: '10px',
                fontWeight: 'bold',
                textDecorationColor: underlineColor,
              }}
            >
              Articles
            </Link>
          )}
          {approvedSalesman && (
            <Link
              underline={
                location.pathname.includes('new-article') ? 'always' : 'hover'
              }
              component={RouterLink}
              to='/new-article'
              sx={{
                fontSize: '22px',
                marginRight: 1,
                paddingLeft: '10px',
                fontWeight: 'bold',
                textDecorationColor: underlineColor,
              }}
            >
              New article
            </Link>
          )}
          {role === 'admin' && (
            <Link
              underline={
                location.pathname.includes('salesmans') ? 'always' : 'hover'
              }
              component={RouterLink}
              to='/salesmans'
              sx={{
                fontSize: '22px',
                marginRight: 1,
                paddingLeft: '10px',
                fontWeight: 'bold',
                textDecorationColor: underlineColor,
              }}
            >
              All salesmans
            </Link>
          )}
          {role === 'admin' && (
            <Link
              underline={
                location.pathname.includes('orders') ? 'always' : 'hover'
              }
              component={RouterLink}
              to='/orders'
              sx={{
                fontSize: '22px',
                marginRight: 1,
                paddingLeft: '10px',
                fontWeight: 'bold',
                textDecorationColor: underlineColor,
              }}
            >
              All orders
            </Link>
          )}
          {!isLoggedin && (
            <Link
              component={RouterLink}
              to='/login'
              underline={
                location.pathname.includes('login') ? 'always' : 'hover'
              }
              sx={{
                fontSize: '22px',
                marginRight: 2,
                paddingLeft: '10px',
                fontWeight: 'bold',
                textDecorationColor: underlineColor,
              }}
            >
              Log in
            </Link>
          )}
          {!isLoggedin && (
            <Link
              component={RouterLink}
              to='/register'
              underline={
                location.pathname.includes('register') ? 'always' : 'hover'
              }
              sx={{
                fontSize: '22px',
                marginRight: 1,
                paddingLeft: '10px',
                fontWeight: 'bold',
                textDecorationColor: underlineColor,
              }}
            >
              Sign up
            </Link>
          )}
         
          {isLoggedin && (
            <Link
              component={RouterLink}
              to='/profile'
              underline={
                location.pathname.includes('profile') ? 'always' : 'hover'
              }
              sx={{
                fontSize: '22px',
                marginRight: 1,
                paddingLeft: '10px',
                fontWeight: 'bold',
                textDecorationColor: underlineColor,
              }}
            >
              Profile
            </Link>
          )}
          {isLoggedin && (
            <Link
              underline='hover'
              component={RouterLink}
              to='/login'
              sx={{
                fontSize: '22px',
                marginRight: 1,
                paddingLeft: '10px',
                fontWeight: 'bold',
              }}
              onClick={(e) => {
                userContext.handleLogout();
              }}
            >
              Log out
            </Link>
          )}
        </Toolbar>
      </AppBar>
    </Box>
  );
};

export default Navbar;
