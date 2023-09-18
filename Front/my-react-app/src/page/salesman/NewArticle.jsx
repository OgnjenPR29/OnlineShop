import { useState, useEffect } from 'react';

import {
  Container,
  Paper,
  Box,
  TextField,
  Typography,
  Button,
} from '@mui/material';


import useServices from '../../services/useServices';
import UploadButton from '../../components/UploadButton';

const NewArticle = () => {
  const [article, setArticle] = useState(articleInit);
  const [image, setProductImage] = useState(null);
  const [validity, setValidity] = useState(fieldValidity);
  const { postArticleRequest, clearRequest, isLoading, error, statusCode } =
    useServices();

  useEffect(() => {
    if (isLoading) {
      return;
    } else if (statusCode === 200 && !error) {
      clearRequest();
      alert('Successfully added a new article!');
    } else if (statusCode !== 200 && error) {
      alert(statusCode, error);
      clearRequest();
    }
  }, [isLoading, statusCode, error, clearRequest]);

  const handleAddArticle = (event) => {
    event.preventDefault();

    setValidity(validateFields(article));

    for (const field in validity) {
      if (validity[field].error) {
        return;
      }
    }

    const requestBody = {
      ...article,
      image,
    };

    postArticleRequest(requestBody);
  };

  return (
    <>
      <Container
        sx={{
          gap: '40px',
          display: 'flex'
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
            marginTop: '79px',
            backgroundColor:'white'
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
              setProductImage(null);
            }}
            uploadCallback={(file) => {
              setProductImage(file);
            }}
          />
        </Box>
        <Paper  elevation={4} sx={{width:'800px', marginTop:'80px'}}>
          <Typography variant='h4'sx={{ marginTop:'5px', marginBottom:'5px', marginLeft:'40%'}}>New Article</Typography>
          <TextField
            id='name'
            label='Name'
            value={article?.name}
            error={validity.name.error}
            helperText={validity.name.helper}
            sx={{ width: '100%', marginBottom:'10px' }}
            onChange={(e) => {
              setArticle((old) => {
                return {
                  ...old,
                  name: e.target.value,
                };
              });
            }}
          />
          <Box sx={{ width: '100%',marginBottom:'10px'}}>
            <TextField
              id='description'
              label='Description'
              value={article?.description}
              error={validity.description.error}
              helperText={validity.description.helper}
              sx={{ width: '100%' }}
              onChange={(e) => {
                setArticle((old) => {
                  return { ...old, description: e.target.value };
                });
              }}
              multiline
            />
          </Box>
          <Box sx={{ width: '100%',  marginBottom:'10px' }}>
            <TextField
              id='quantity'
              label='Quantity'
              type='number'
              value={article?.quantity}
              sx={{ width: '100%' }}
              onChange={(e) => {
                var quantity = e.target.value;
                if (quantity < 1) {
                  quantity = 1;
                }
                setArticle((old) => {
                  return { ...old, quantity: quantity };
                });
              }}
            />
          </Box>
          <Box sx={{ width: '100%', marginBottom:'10px' }}>
            <TextField
              id='price'
              label='Price'
              type='number'
              value={article?.price}
              sx={{ width: '100%' }}
              onChange={(e) => {
                setArticle((old) => {
                  var price = e.target.value;
                  if (price < 1) {
                    price = 1;
                  }
                  return { ...old, price: price };
                });
              }}
            />
          </Box>
          <Button
            sx={{ width: '100%', marginTop: '15px', alignSelf: 'center' }}
            variant='contained'
            type='submit'
            onClick={(event) => {
              handleAddArticle(event);
            }}
          >
            Add article
          </Button>
        </Paper>
      </Container>
    </>
  );
};

const fieldValidity = {
  name: {
    error: false,
    helper: '',
  },
  description: {
    error: false,
    helper: '',
  },
};

const articleInit = {
  name: '',
  description: '',
  quantity: 0,
  price: 1,
};

const validateFields = (article) => {
  const updatedFieldValidity = { ...fieldValidity };

  const requiredFields = Object.keys(fieldValidity);

  requiredFields.forEach((field) => {
    if (!article[field]) {
      updatedFieldValidity[field].error = true;
      updatedFieldValidity[field].helper = 'Field is required';
    } else if (article[field].length < 3) {
      updatedFieldValidity[field].error = true;
      updatedFieldValidity[field].helper = 'Too short';
    } else {
      updatedFieldValidity[field].error = false;
      updatedFieldValidity[field].helper = '';
    }
  });

  return updatedFieldValidity;
};

export default NewArticle;
