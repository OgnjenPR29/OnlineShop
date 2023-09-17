import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import { BrowserRouter } from 'react-router-dom';
import {ThemeProvider } from '@emotion/react';
import {  createTheme, CssBaseline } from '@mui/material';
import ContextProvider from './context/ContextProvider';
import { GoogleOAuthProvider } from '@react-oauth/google';
import { UserContextProvider } from './context/UserContext'; 

const theme = createTheme({
  palette: {
    primary: {
      main: '#007bff', // Set your primary color
    },
    secondary: {
      main: '#ff5722', // Set your secondary color
    },
  },
  // Customize other theme properties as needed
});

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
     <BrowserRouter>
      <ContextProvider>
        <GoogleOAuthProvider clientId='19815919039-igkkmbt256ijmj205mgp6o5uch8esbp2.apps.googleusercontent.com'>
          <App />
        </GoogleOAuthProvider>
      </ContextProvider>
    </BrowserRouter>
);
