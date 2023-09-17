import { UserContextProvider } from './UserContext';
import { OrderContextProvider } from './OrderContext';


const ContextProvider = ({ children }) => {
  return (
    <UserContextProvider>
      <OrderContextProvider>{children}</OrderContextProvider>
    </UserContextProvider>
  );
};

export default ContextProvider;
