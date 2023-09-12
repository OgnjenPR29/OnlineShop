import { Link as RouterLink, useNavigate } from 'react-router-dom';
import { useState, useEffect, useContext, useRef } from 'react';

import {
  Box,
  Button,
  Container,
  Link,
  Paper,
  TextField,
  Typography,
} from '@mui/material';

import UserContext from '../../context/UserContext';
import useServices from '../../services/useServices';
import GoogleLoginApi from '../../components/GoogleLogin';

const Login = () => {
  const user = useRef(userInit);
  const [validity, setValidity] = useState(fieldValidity);
  const { loginRequest, isLoading, error, statusCode, data } = useServices();
  const userContext = useContext(UserContext);
  const navigate = useNavigate();

  const handleSubmit = (event) => {
    event.preventDefault();

    setValidity(validateFields(user.current));

    for (const field in validity) {
      if (validity[field].error) {
        return;
      }
    }

    loginRequest(user.current);
  };

  const handleError = (statusCode, errorMessage) => {
    alert('Status code: ' + statusCode + '\n' + errorMessage);
  };
  
    const handleSuccess = (message) => {
    alert(message);
    };

  useEffect(() => {
    if (isLoading) {
      return;
    } else if (statusCode === 200 && !error) {
      handleSuccess('Successfuly logged in!');
    } else if (statusCode !== 200 && error) {
      handleError(statusCode, error);
    }
  }, [isLoading, statusCode, error]);

  useEffect(() => {
    if (data && statusCode === 200 && !userContext.isLoggedin) {
      userContext.handleLogin(data);
      navigate('/');
    }
  }, [statusCode, data, userContext, navigate]);

  return (
    <>
      <Container sx={{
        display:'flex',
        padding:'30px',
        alignItems: 'center', 
        justifyContent: 'center',
        height: '80vh', }}>
        <Paper
          component='form'
          sx={{ width: '30%' }}
          elevation={4}
        >
          <TextField
            placeholder='Username'
            value={user.username}
            sx={{ width: '85%', margin:'20px'}}
            error={validity.username.error}
            helperText={validity.username.helper}
            onChange={(e) => {
              user.current.username = e.target.value;
            }}
          />
          <TextField
            placeholder='Password'
            type='password'
            value={user.password}
            sx={{ width: '85%', margin:'20px'}}
            error={validity.password.error}
            helperText={validity.password.helper}
            onChange={(e) => {
              user.current.password = e.target.value;
            }}
          />
          <Button
            sx={{ marginTop: '20px', marginLeft:'85px', width: '50%' }}
            variant='contained'
            onClick={(event) => {
              handleSubmit(event);
            }}
          >
            Sign in
          </Button>
          <Typography sx={{ marginTop: '15px', marginLeft:'35px' }}>
            Don't have an account? Sign up..
            <Link component={RouterLink} to='/register' underline='hover'>
              here
            </Link>
            !
          </Typography>
          <Box sx={{ margin: '15px', marginLeft:'35px' }}>
            <GoogleLoginApi />
          </Box>
        </Paper>
      </Container>
    </>
  );
};

var userInit = {
  username: '',
  password: '',
};

const fieldValidity = {
  username: {
    error: false,
    helper: '',
  },
  password: {
    error: false,
    helper: '',
  },
};

const validateFields = (user) => {
  const updatedFieldValidity = { ...fieldValidity };

  const requiredFields = Object.keys(fieldValidity);

  requiredFields.forEach((field) => {
    if (!user[field]) {
      updatedFieldValidity[field].error = true;
      updatedFieldValidity[field].helper = 'Field is required';
    } else if (user[field].length < 3) {
      updatedFieldValidity[field].error = true;
      updatedFieldValidity[field].helper = 'Too short';
    } else {
      updatedFieldValidity[field].error = false;
      updatedFieldValidity[field].helper = '';
    }
  });

  return updatedFieldValidity;
};

export default Login;
