import { useCallback } from 'react';
import useHttp from './/useHttp';

const baseUrl = process.env.REACT_APP_API_BASE_URL;
const registerUrl = baseUrl + process.env.REACT_APP_API_REGISTER_URL;
const loginUrl = baseUrl + process.env.REACT_APP_API_LOGIN_URL;
const getUserUrl = baseUrl + process.env.REACT_APP_API_GET_USER_URL;
const updateUserUrl = baseUrl + process.env.REACT_APP_API_UPDATE_USER_URL;
const changePasswordUrl =
  baseUrl + process.env.REACT_APP_API_CHANGE_PASSWORD_URL;
const getProfileImageUrl =
  baseUrl + process.env.REACT_APP_API_GET_PROFILE_IMAGE_URL;
const changeProfileImageUrl =
  baseUrl + process.env.REACT_APP_API_CHANGE_PROFILE_IMAGE_URL;
const getAllSellersUrl =
  baseUrl + process.env.REACT_APP_API_ADMIN_ALL_SELLERS_URL;
const updateSellerStatusUrl =
  baseUrl + process.env.REACT_APP_API_UPDATE_APPROVAL_STATUS_URL;
const allOrdersUrl = baseUrl + process.env.REACT_APP_API_ALL_ORDERS_URL;
const getSellersFinishedOrdersUrl =
  baseUrl + process.env.REACT_APP_API_SELLER_FINISHED_ORDERS_URL;
const getSellersPendingOrdersUrl =
  baseUrl + process.env.REACT_APP_API_SELLER_PENDING_ORDERS_URL;
const getSellersOrderDetailsUrl =
  baseUrl + process.env.REACT_APP_API_ORDER_DETAILS_URL;
const getSellerArticleDetailsUrl =
  baseUrl + process.env.REACT_APP_API_SELLER_GET_ARTICLE_DETAILS_URL;
const getSellerArticlesUrl =
  baseUrl + process.env.REACT_APP_API_SELLER_ARTICLES_URL;
const updateArticleUrl =
  baseUrl + process.env.REACT_APP_API_SELLER_UPDATE_ARTICLE_URL;
const updateArticleProductImageUrl =
  baseUrl + process.env.REACT_APP_API_SELLER_UPDATE_ARTICLE_IMAGE_URL;
const deleteArticleUrl =
  baseUrl + process.env.REACT_APP_API_SELLER_DELETE_ARTICLE_URL;
const postArticleUrl =
  baseUrl + process.env.REACT_APP_API_SELLER_POST_ARTICLE_URL;
const customerFinishedOrdersUrl =
  baseUrl + process.env.REACT_APP_API_CUSTOMER_FINISHED_ORDERS_URL;
const customerPendingOrdersUrl =
  baseUrl + process.env.REACT_APP_API_CUSTOMER_PENDING_ORDERS_URL;
const customerArticlesUrl =
  baseUrl + process.env.REACT_APP_API_CUSTOMER_ARTICLES_URL;
const customerOrderUrl = baseUrl + process.env.REACT_APP_API_CUSTOMER_ORDER_URL;
const customerDeleteOrderUrl =
  baseUrl + process.env.REACT_APP_API_CUSTOMER_DELETE_ORDER;
const customerPostOrderUrl =
  baseUrl + process.env.REACT_APP_API_CUSTOMER_POST_ORDER_URL;
const googleLoginUrl = baseUrl + process.env.REACT_APP_API_GOOGLE_LOGIN;

const useServices = () => {
    const {
      data,
      isLoading,
      error,
      statusCode,
      getRequest,
      postRequest,
      postRequestFormData,
      putRequest,
      putRequestFormData,
      deleteRequest,
      resetHttp,
    } = useHttp();

    const registerRequest = useCallback(
        (user) => {
          postRequestFormData(registerUrl, user);
        },
        [postRequestFormData]
      );
    
    const loginRequest = useCallback(
      (credentials) => {
        postRequest(loginUrl, credentials);
      },
      [postRequest]
    );
  
    const getUserProfileRequest = useCallback(() => {
      getRequest(getUserUrl);
    }, [getRequest]);
  
    const updateUserRequest = useCallback(
      (user) => {
        putRequest(updateUserUrl, user);
      },
      [putRequest]
    );
  
    const changePasswordRequest = useCallback(
      (data) => {
        putRequest(changePasswordUrl, data);
      },
      [putRequest]
    );
  
    const getProfileImageRequest = useCallback(() => {
      getRequest(getProfileImageUrl);
    }, [getRequest]);
  
    const updateProfileImageRequest = useCallback(
      (data) => {
        putRequestFormData(changeProfileImageUrl, data);
      },
      [putRequestFormData]
    );

      return {
        data,
        isLoading,
        error,
        statusCode,
        clearRequest: resetHttp,
        registerRequest,
        loginRequest,
        getUserProfileRequest,
        updateUserRequest,
        changePasswordRequest,
        getProfileImageRequest,
        updateProfileImageRequest
      };

    };

export default useServices;
