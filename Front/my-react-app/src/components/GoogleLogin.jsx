import { GoogleLogin } from '@react-oauth/google';
import useServices from '../services/useServices';

const handleError = (statusCode, errorMessage) => {
    alert('Status code: ' + statusCode + '\n' + errorMessage);
    };

function handleLoginSuccess(response) {
  const idToken = response.tokenId;
}

function handleLoginFailure(error) {
  handleError('Google login failed:', error);
}



function GoogleLoginApi() {
  const { google } = useServices();

  return (
    <GoogleLogin
      buttonText='Login with Google'
      onSuccess={handleLoginSuccess}
      onFailure={handleLoginFailure}
    />
  );
}

export default GoogleLoginApi;
