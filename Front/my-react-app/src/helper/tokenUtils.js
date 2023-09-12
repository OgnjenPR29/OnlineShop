import jwtDecode from 'jwt-decode';

// Returns decoded token
export const getToken = () => {
  const token = window.localStorage.getItem('token');

  if (!token) {
    return null;
  }

  try {
    const decoded = jwtDecode(token);

    return decoded;
  } catch (exception) {
  }
};

const isTokenExpired = () => {
  const token = getToken();

  const expirationTime = token.exp;
  const currentTime = Math.floor(Date.now() / 1000);

  return expirationTime < currentTime;
};

export const isLoggedin = () => {
  const token = window.localStorage.getItem('token');

  if (!token) {
    return false;
  }

  const decoded = getToken();

  if (!decoded) {
    return false;
  }

  return true;
};

// Gets token JSON object
export const getRawToken = () => {
  const token = window.localStorage.getItem('token');
  const tokenJson = token && JSON.parse(token)?.token;

  return tokenJson;
};

export const getRole = () => {
  const token = getToken();

  return token?.role;
};

export const getId = () => {
  const token = getToken();

  return token?.id;
};

export const getUsername = () => {
  const token = getToken();

  return token?.username;
};

export const getStatus = () => {
  const token = getToken();

  return token?.status;
};

export const getUser = () => {
  const user = {
    username: getUsername(),
    role: getRole(),
    id: getId(),
    status: getStatus(),
    rawToken: getRawToken(),
  };

  return user;
};

// Takes token as Object and saves it as stringified JSON
export const saveToken = (tokenObj) => {
  if (!tokenObj) {
    return;
  }

  const token = JSON.stringify(tokenObj);
  window.localStorage.setItem('token', token);
};

export const removeToken = () => {
  window.localStorage.removeItem('token');
};

const tokenUtils = {
  getToken,
  isTokenExpired,
  isLoggedin,
  getRawToken,
  getRole,
  getId,
  getUsername,
  getStatus,
  getUser,
  saveToken,
  removeToken,
};

export default tokenUtils;
