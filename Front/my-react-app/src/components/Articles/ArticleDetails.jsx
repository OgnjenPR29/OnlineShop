import { useState, useEffect, useContext } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

import {
  Container,
  Paper,
  Box,
  TextField,
  Typography,
  Button,
} from '@mui/material';


import NoData from '../NoData';
import useServices from '../../services/useServices';
import UploadButton from '../UploadButton';
import UserContext from '../../context/UserContext';

const ArticleDetails = () => {
  const [article, setArticle] = useState(null);
  const [fetchingArticle, setFetchingArticle] = useState(true);
  const [updatingProductImage, setUpdatingProductImage] = useState(false);
  const [updatingArticle, setUpdatingArticle] = useState(false);
  const [deletingArticle, setDeletingArticle] = useState(false);
  const [image, setProductImage] = useState(false);
  const userContext = useContext(UserContext);
  const { name } = useParams();
  const {
    getSalesmanArticleDetailsRequest,
    updateArticleProductImageRequest,
    updateArticleRequest,
    clearRequest,
    deleteArticleRequest,
    isLoading,
    error,
    statusCode,
    data,
  } = useServices();
  const navigate = useNavigate();

  useEffect(() => {
    getSalesmanArticleDetailsRequest(name);
  }, [getSalesmanArticleDetailsRequest, name]);

  const role = userContext.isLoggedin && userContext.role.toLowerCase();

  useEffect(() => {
    if (isLoading) {
      return;
    } else if (statusCode === 200 && !error && data && fetchingArticle) {
      data.image &&
        setProductImage('data:image/*;base64,' + data.image);
      setArticle({ ...data, newName: data.name });
      setFetchingArticle(false);
      console.log(data)
      clearRequest();
    } else if (statusCode === 200 && !error && updatingArticle) {
      setUpdatingArticle(false);
      setFetchingArticle(true);
      clearRequest();
      getSalesmanArticleDetailsRequest(article.newName);
      alert('Successfully updated article info!');
    } else if (statusCode === 200 && !error && updatingProductImage) {
      setUpdatingProductImage(false);
      setFetchingArticle(true);
      clearRequest();
      getSalesmanArticleDetailsRequest(name);
      alert('Successfully updated product image!');
    } else if (statusCode === 200 && !error && deletingArticle) {
      setDeletingArticle(false);
      clearRequest();
      alert('Successfully deleted the article!');
      navigate('/articles');
    } else if (statusCode !== 200 && error) {
      alert(statusCode, error);
      clearRequest();
    }
  }, [
    isLoading,
    statusCode,
    error,
    data,
    clearRequest,
    updatingProductImage,
    updatingArticle,
    fetchingArticle,
    getSalesmanArticleDetailsRequest,
    name,
  ]);

  return (
    <>
      {article && (
        <Container
          sx={{
            gap: '40px',
            display:'flex',
            marginTop: '80px',
            justifyContent:'center'
          }}
        >
          <Box
            sx={{
              display: 'flex',
              flexDirection: 'column',
              gap: '25px',
              justifyItems: 'center',
              alignItems: 'center',
              width: '30%',
              borderRadius: '3px',
              padding: '10px',
              boxShadow:
                '0px 2px 4px -1px rgba(0,0,0,0.2), 0px 4px 5px 0px rgba(0,0,0,0.14), 0px 1px 10px 0px rgba(0,0,0,0.12)',
              marginTop: '0px',
            }}
          >
            <Typography sx={{ fontSize: '24px', padding: '0px' }}>
              Product image
            </Typography>
            <UploadButton
              width='100%'
              maxHeightPerc='250px'
              maxWidthPerc='100%'
              image={image}
              buttonText='Reupload'
              direction='column'
              alternativeToNoImage='No product image...'
              doubleClickCallback={() => {
                updateArticleProductImageRequest({ Image: null, name });
                setProductImage(null);
                setUpdatingProductImage(true);
              }}
              uploadCallback={(file) => {
                updateArticleProductImageRequest({ Image: file, name });
                setProductImage(file);
                setUpdatingProductImage(true);
              }}
            />
          </Box>
          <Paper  elevation={4} sx={{width:'500px'}}>
            <Typography variant='h4' sx={{margin:'10px',marginLeft:'40%'}}>Article</Typography>
            <TextField
              id='name'
              label='Name'
              value={article?.newName}
              sx={{  width: '100%',marginBottom:'15px' }}
              onChange={(e) => {
                setArticle((old) => {
                  return {
                    ...old,
                    newName: e.target.value,
                  };
                });
              }}
            />
            <Box sx={{  width: '100%' }}>
              <TextField
                id='description'
                label='Description'
                value={article?.description}
                sx={{ width: '100%', marginBottom:'15px' }}
                onChange={(e) => {
                  setArticle((old) => {
                    return { ...old, description: e.target.value };
                  });
                }}
                multiline
              />
            </Box>
            <Box sx={{ width: '100%' }}>
              <TextField
                id='quantity'
                label='Quantity'
                value={article?.quantity}
                sx={{ width: '100%',marginBottom:'15px' }}
                onChange={(e) => {
                  setArticle((old) => {
                    return { ...old, quantity: e.target.value };
                  });
                }}
              />
            </Box>
            <Box sx={{ width: '100%' }}>
              <TextField
                id='price'
                label='Price'
                value={article.price}
                sx={{ width: '100%' }}
                onChange={(e) => {
                  setArticle((old) => {
                    return { ...old, price: e.target.value };
                  });
                }}
              />
            </Box>
            <Box sx={{ width: '100%' }}>
              {role === 'salesman' && (
                <Button
                  sx={{ width: '50%', marginTop: '15px', alignSelf: 'center' }}
                  variant='contained'
                  type='submit'
                  onClick={(event) => {
                    setUpdatingArticle(true);
                    updateArticleRequest({
                      ...article,
                      currentName: article.name,
                    });
                  }}
                >
                  Update
                </Button>
              )}
              {role === 'salesman' && (
                <Button
                  sx={{ width: '50%', marginTop: '15px', alignSelf: 'center' }}
                  variant='contained'
                  color='secondary'
                  type='submit'
                  onClick={(event) => {
                    deleteArticleRequest(article.name);
                    setDeletingArticle(true);
                  }}
                >
                  Delete
                </Button>
              )}
            </Box>
          </Paper>
        </Container>
      )}
      {!isLoading && !article && <NoData>Article not found...</NoData>}
    </>
  );
};

export default ArticleDetails;
