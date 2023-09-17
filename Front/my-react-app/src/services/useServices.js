import { useCallback } from 'react';
import useHttp from './/useHttp';

const baseUrl = process.env.REACT_APP_API_BASE_URL;
const registerUrl = baseUrl + process.env.REACT_APP_API_REGISTER_URL;
const loginUrl = baseUrl + process.env.REACT_APP_API_LOGIN_URL;
const getUserUrl = baseUrl + process.env.REACT_APP_API_GET_USER_URL;
const updateUserUrl = baseUrl + process.env.REACT_APP_API_UPDATE_USER_URL;
const changePasswordUrl = baseUrl + process.env.REACT_APP_API_CHANGE_PASSWORD_URL;
const getProfileImageUrl = baseUrl + process.env.REACT_APP_API_GET_PROFILE_IMAGE_URL;
const changeProfileImageUrl = baseUrl + process.env.REACT_APP_API_CHANGE_PROFILE_IMAGE_URL;
const getAllSalesmansUrl = baseUrl + process.env.REACT_APP_API_ADMIN_ALL_SALESMANS_URL;
const updateSalesmanStatusUrl = baseUrl + process.env.REACT_APP_API_UPDATE_APPROVAL_STATUS_URL;
const allOrdersUrl = baseUrl + process.env.REACT_APP_API_ALL_ORDERS_URL;
const getSalesmansFinishedOrdersUrl = baseUrl + process.env.REACT_APP_API_SALESMAN_FINISHED_ORDERS_URL;
const getSalesmansPendingOrdersUrl = baseUrl + process.env.REACT_APP_API_SALESMAN_PENDING_ORDERS_URL;
const getSalesmansOrderDetailsUrl = baseUrl + process.env.REACT_APP_API_ORDER_DETAILS_URL;
const getSalesmanArticleDetailsUrl = baseUrl + process.env.REACT_APP_API_SALESMAN_GET_ARTICLE_DETAILS_URL;
const getSalesmanArticlesUrl = baseUrl + process.env.REACT_APP_API_SALESMAN_ARTICLES_URL;
const updateArticleUrl = baseUrl + process.env.REACT_APP_API_SALESMAN_UPDATE_ARTICLE_URL;
const updateArticleProductImageUrl = baseUrl + process.env.REACT_APP_API_SALESMAN_UPDATE_ARTICLE_IMAGE_URL;
const deleteArticleUrl = baseUrl + process.env.REACT_APP_API_SALESMAN_DELETE_ARTICLE_URL;
const postArticleUrl = baseUrl + process.env.REACT_APP_API_SALESMAN_POST_ARTICLE_URL;
const shopperFinishedOrdersUrl =  baseUrl + process.env.REACT_APP_API_SHOPPER_FINISHED_ORDERS_URL;
const shopperPendingOrdersUrl = baseUrl + process.env.REACT_APP_API_SHOPPER_PENDING_ORDERS_URL;
const shopperArticlesUrl = baseUrl + process.env.REACT_APP_API_SHOPPER_ARTICLES_URL;
const shopperOrderUrl = baseUrl + process.env.REACT_APP_API_SHOPPER_ORDER_URL;
const shopperDeleteOrderUrl = baseUrl + process.env.REACT_APP_API_SHOPPER_DELETE_ORDER;
const shopperPostOrderUrl = baseUrl + process.env.REACT_APP_API_SHOPPER_POST_ORDER_URL;
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

    const getAllSalesmansRequest = useCallback(() => {
      getRequest(getAllSalesmansUrl);
    }, [getRequest]);

    const updateSalesmanStatusRequest = useCallback(
      (data) => {
        putRequest(updateSalesmanStatusUrl, data);
      },
      [putRequest]
    );

    const getAllOrdersRequest = useCallback(() => {
      getRequest(allOrdersUrl);
    }, [getRequest]);

    const getAdminOrderDetailsRequest = useCallback(
      (id) => {
        getRequest(baseUrl + '/admin/order' + '?id=' + id);
      },
      [getRequest]
    );

    const getSalesmansFinishedOrders = useCallback(() => {
      getRequest(getSalesmansFinishedOrdersUrl);
    }, [getRequest]);
  
    const getSalesmansPendingOrders = useCallback(() => {
      getRequest(getSalesmansPendingOrdersUrl);
    }, [getRequest]);
  
    const getSalesmansOrderDetailsRequest = useCallback(
      (id) => {
        getRequest(getSalesmansOrderDetailsUrl + '?id=' + id);
      },
      [getRequest]
    );
  
    const getSalesmanArticleDetailsRequest = useCallback(
      (name) => {
        getRequest(getSalesmanArticleDetailsUrl + '?name=' + name);
      },
      [getRequest]
    );
  
    const getSalesmansArticlesRequest = useCallback(() => {
      getRequest(getSalesmanArticlesUrl);
    }, [getRequest]);
  
    const updateArticleRequest = useCallback(
      (article) => {
        console.log(article, updateArticleUrl);
        putRequest(updateArticleUrl, article);
      },
      [putRequest]
    );
  
    const updateArticleProductImageRequest = useCallback(
      (article) => {
        putRequestFormData(updateArticleProductImageUrl, article);
      },
      [putRequestFormData]
    );
  
    const postArticleRequest = useCallback(
      (article) => {
        postRequestFormData(postArticleUrl, article);
      },
      [postRequestFormData]
    );

    const deleteArticleRequest = useCallback(
      (name) => {
        deleteRequest(deleteArticleUrl + '?name=' + name);
      },
      [deleteRequest]
    );

    const getShopperFinishedOrdersRequest = useCallback(() => {
      getRequest(shopperFinishedOrdersUrl);
    }, [getRequest]);
  
    const getShopperPendingOrdersRequest = useCallback(() => {
      getRequest(shopperPendingOrdersUrl);
    }, [getRequest]);
  
    const getShopperArticlesRequest = useCallback(() => {
      getRequest(shopperArticlesUrl);
    }, [getRequest]);
  
    const getShopperOrderDetailsRequest = useCallback(
      (id) => {
        getRequest(shopperOrderUrl + '?id=' + id);
      },
      [getRequest]
    );
  
    const postShopperOrderRequest = useCallback(
      (order) => {
        postRequest(shopperPostOrderUrl, order);
      },
      [postRequest]
    );
  
    const deleteShopperOrderRequest = useCallback(
      (orderId) => {
        deleteRequest(shopperDeleteOrderUrl + '?orderId=' + orderId);
      },
      [deleteRequest]
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
        updateProfileImageRequest,
        getAllSalesmansRequest,
        updateSalesmanStatusRequest,
        getAllOrdersRequest,
        getAdminOrderDetailsRequest,
        getSalesmansFinishedOrders,
        getSalesmansPendingOrders,
        getSalesmansOrderDetailsRequest,
        getSalesmanArticleDetailsRequest,
        getSalesmansArticlesRequest,
        updateArticleProductImageRequest,
        postArticleRequest,
        updateArticleRequest,
        deleteArticleRequest,
        getShopperFinishedOrdersRequest,
        getShopperPendingOrdersRequest,
        getShopperArticlesRequest,
        getShopperOrderDetailsRequest,
        postShopperOrderRequest,
        deleteShopperOrderRequest
      };

    };

export default useServices;
