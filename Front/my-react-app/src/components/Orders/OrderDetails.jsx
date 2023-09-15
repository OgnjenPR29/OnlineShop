import { useState, useEffect, useContext } from 'react';
import { useParams } from 'react-router-dom';

import { Container, Paper, Box, TextField, Typography } from '@mui/material';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';

import NoData from '../NoData';
import useServices from '../../services/useServices';
import { getDateString } from '../../helper/dateTimeUtils';
import UserContext from '../../context/UserContext';

const OrderDetails = () => {
  const [order, setOrder] = useState(null);
  const { id } = useParams();
  const {
    getAdminOrderDetailsRequest,
    getSellersOrderDetailsRequest,
    getCustomerOrderDetailsRequest,
    clearRequest,
    isLoading,
    error,
    statusCode,
    data,
  } = useServices();
  const { role } = useContext(UserContext);

  useEffect(() => {
    role.toLowerCase() === 'admin' && getAdminOrderDetailsRequest(id);
    role.toLowerCase() === 'seller' && getSellersOrderDetailsRequest(id);
    role.toLowerCase() === 'customer' && getCustomerOrderDetailsRequest(id);
  }, [
    getAdminOrderDetailsRequest,
    getSellersOrderDetailsRequest,
    getCustomerOrderDetailsRequest,
    id,
    role,
  ]);

  useEffect(() => {
    if (isLoading) {
      return;
    } else if (statusCode === 200 && !error && data) {
      data?.items.forEach((item) => {
        item.articleImage = 'data:image/*;base64,' + item.articleImage;
      });
      setOrder(data);
      clearRequest();
    } else if (statusCode !== 200 && error) {
      alert(statusCode, error);
      clearRequest();
    }
  }, [isLoading, statusCode, error, data, clearRequest]);

  return (
    <>
      {order && (
        <Container >
          <Paper elevation={4}>
            <Typography variant='h4'>Order {order?.id}</Typography>
            <Box sx={{  width: '100%' }}>
              <TextField
                id='comment'
                label='Comment'
                value={order?.comment}
                multiline
                rows={3}
                disabled
                sx={{
                  width: '100%',
                  '.MuiInputBase-input.Mui-disabled': {
                    WebkitTextFillColor: 'white',
                  },
                }}
                aria-readonly='true'
                variant='outlined'
              />
            </Box>
            <Box>
              <TextField
                id='address'
                label='Address'
                value={order?.address}
                disabled
                sx={{
                  width: '100%',
                  '.MuiInputBase-input.Mui-disabled': {
                    WebkitTextFillColor: 'white',
                  },
                }}
                aria-readonly='true'
                variant='outlined'
              />
              <TextField
                id='totalPrice'
                label='Total price'
                value={order?.totalPrice}
                disabled
                sx={{
                  width: '100%',
                  '.MuiInputBase-input.Mui-disabled': {
                    WebkitTextFillColor: 'white',
                  },
                }}
                aria-readonly='true'
                variant='outlined'
              />
            </Box>
            <Box >
              <TextField
                id='placedTime'
                label='Placed time'
                value={getDateString(order?.placedTime)}
                disabled
                sx={{
                  width: '100%',
                  '.MuiInputBase-input.Mui-disabled': {
                    WebkitTextFillColor: 'white',
                  },
                }}
                aria-readonly='true'
                variant='outlined'
              />
              <TextField
                id='remainingTime'
                label='Remaining time'
                value={order?.remainingTime}
                disabled
                sx={{
                  width: '100%',
                  '.MuiInputBase-input.Mui-disabled': {
                    WebkitTextFillColor: 'white',
                  },
                }}
                aria-readonly='true'
                variant='outlined'
              />
            </Box>
          </Paper>
        </Container>
      )}
      {order?.items.length > 0 && (
        <Container
          sx={{
            marginTop: '30px',
          }}
        >
          <TableContainer component={Paper} elevation={4} sx={{ width: '80%' }}>
            <Table
              sx={{ padding: '5px', width: '100%' }}
              aria-label='simple table'
            >
              <TableHead>
                <TableRow>
                  <TableCell align='center'>ArticleId </TableCell>
                  <TableCell align='center'>Article image</TableCell>
                  <TableCell align='center'>Article name</TableCell>
                  <TableCell align='center'>Price per unit</TableCell>
                  <TableCell align='center'>Quantity</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {order?.items.map((row) => (
                  <TableRow
                    key={row.articleId}
                    sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                  >
                    <TableCell component='th' align='center' scope='row'>
                      {row.articleId}
                    </TableCell>
                    <TableCell align='center'>
                      <Box
                        sx={{
                          display: 'flex',
                          justifyContent: 'center',
                          alignItems: 'center',
                        }}
                      >
                        <img
                          src={row.articleImage}
                          alt=''
                          style={{ maxWidth: '120px', maxHeight: '120px' }}
                        />
                      </Box>
                    </TableCell>
                    <TableCell align='center'>{row.articleName}</TableCell>
                    <TableCell align='center'>{row.pricePerUnit}</TableCell>
                    <TableCell align='center'>{row.quantity}</TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </Container>
      )}
      {!order && !isLoading && <NoData>Order {id} not found...</NoData>}
      {order && order?.items.length === 0 && !isLoading && (
        <NoData>No items to show...</NoData>
      )}
    </>
  );
};

export default OrderDetails;
