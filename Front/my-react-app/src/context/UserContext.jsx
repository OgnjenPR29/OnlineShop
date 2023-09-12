import { createContext, useCallback, useState } from 'react';

import tokenUtils from '../helper/tokenUtils';

const UserContext = createContext({
  handleLogin: () => {},
  handleLogout: () => {},
  loadUser: () => {},
  isLoggedin: false,
  username: '',
  role: '',
  status: null,
  rawToken: '',
});

export const UserContextProvider = ({ children }) => {
  const [isLoggedin, setIsLoggedin] = useState(false);
  const [user, setUser] = useState(userInit);

  const handleLogin = useCallback((data) => {
    tokenUtils.saveToken(data);
    setIsLoggedin(tokenUtils.isLoggedin());
    const user = tokenUtils.getUser();
    setUser(user);
  }, []);

  const handleLogout = useCallback(() => {
    tokenUtils.removeToken();
    setIsLoggedin(false);
    setUser(userInit);
  }, []);

  const loadUser = useCallback(() => {
    if (!tokenUtils.isLoggedin()) {
      return;
    }

    if (tokenUtils.isTokenExpired()) {
      tokenUtils.removeToken();
      return;
    }

    setIsLoggedin(tokenUtils.isLoggedin());
    const user = tokenUtils.getUser();
    setUser(user);
  }, []);

  return (
    <UserContext.Provider
      value={{
        handleLogin,
        handleLogout,
        loadUser,
        isLoggedin,
        username: user.username,
        role: user.role,
        rawToken: user.rawToken,
        status: user.status,
      }}
    >
      {children}
    </UserContext.Provider>
  );
};

const userInit = {
  username: '',
  role: '',
  status: null,
  rawToken: '',
};

export default UserContext;
