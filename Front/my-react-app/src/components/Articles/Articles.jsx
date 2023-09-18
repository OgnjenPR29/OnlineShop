import { useContext } from 'react';

import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import { Button, Container, Box } from '@mui/material';


import NoData from '../NoData';
import OrderContext from '../../context/OrderContext';

const Articles = ({ data, hasButton, buttonCallback, buttonText }) => {
  const orderContext = useContext(OrderContext);

  return (
    <>
      {data && data.length > 0 && (
        <Container
          sx={{
            margin: '0,60,0,0',
          }}
        >
          <TableContainer component={Paper} elevation={4} sx={{marginTop:'80px'}}>
            <Table
              sx={{ padding: '5px', width: '100%' }}
              aria-label='simple table'
            >
              <TableHead>
                <TableRow>
                  <TableCell align='center' sx={{ fontSize: '22px' }}>
                    Product image
                  </TableCell>
                  <TableCell align='center' sx={{ fontSize: '22px' }}>
                    Id
                  </TableCell>
                  <TableCell align='center' sx={{ fontSize: '22px' }}>
                    Name
                  </TableCell>
                  <TableCell align='center' sx={{ fontSize: '22px' }}>
                    Description
                  </TableCell>
                  <TableCell align='center' sx={{ fontSize: '22px' }}>
                    Quantity
                  </TableCell>
                  <TableCell align='center' sx={{ fontSize: '22px' }}>
                    Price
                  </TableCell>
                  {hasButton && <TableCell align='center'>Action</TableCell>}
                </TableRow>
              </TableHead>
              <TableBody>
                {data.map((row) => (
                  <TableRow
                    key={row.id}
                    sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                  >
                    <TableCell component='th' align='center' scope='row'>
                      <Box
                        sx={{
                          display: 'flex',
                          justifyContent: 'center',
                          alignItems: 'center',
                        }}
                      >
                        <img
                          src={row.image}
                          alt=''
                          style={{ maxWidth: '140px', maxHeight: '140px' }}
                        />
                      </Box>
                    </TableCell>
                    <TableCell align='center' sx={{ fontSize: '18px' }}>
                      {row.id}
                    </TableCell>
                    <TableCell align='center' sx={{ fontSize: '18px' }}>
                      {row.name}
                    </TableCell>
                    <TableCell align='center' sx={{ fontSize: '18px' }}>
                      {row.description}
                    </TableCell>
                    <TableCell align='center' sx={{ fontSize: '18px' }}>
                      {row.quantity}
                    </TableCell>
                    <TableCell align='center' sx={{ fontSize: '18px' }}>
                      {row.price}
                    </TableCell>
                    {hasButton && !orderContext.hasArticleWithId(row.id) && (
                      <TableCell align='center'>
                        <Button
                          variant='contained'
                          onClick={(e) => {
                            buttonCallback(row);
                          }}
                          style={{ display: row.quantity == 0 && buttonText == 'Add to order' ? 'none' : 'flex' }} 
                        >
                          {buttonText}
                        </Button>
                      </TableCell>
                    )}
                    {hasButton && orderContext.hasArticleWithId(row.id) && (
                      <TableCell align='center'>Article added</TableCell>
                    )}
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </Container>
      )}
      {data.length === 0 && <NoData>No articles to show...</NoData>}
    </>
  );
};

export default Articles;
