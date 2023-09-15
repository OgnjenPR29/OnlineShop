import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import { Button, Container } from '@mui/material';

import NoData from '../NoData';
import { getDateString } from '../../helper/dateTimeUtils';


const Orders = ({
  role,
  data,
  hasButton,
  buttonCallback,
  buttonText,
  cancelOrderCallback,
}) => {
  const userRole = role.toLowerCase();

  return (
    <>
      {data && data.length > 0 && (
        <Container
          sx={{
            margin: '0,60,0,0',
          }}
        >
          <TableContainer component={Paper} elevation={4}>
            <Table
              sx={{ padding: '5px', width: '100%' }}
              aria-label='simple table'
            >
              <TableHead>
                <TableRow>
                  <TableCell align='center' sx={{ fontSize: '18px' }}>
                    Id{' '}
                  </TableCell>
                  <TableCell align='center' sx={{ fontSize: '18px' }}>
                    Comment{' '}
                  </TableCell>
                  <TableCell align='center' sx={{ fontSize: '18px' }}>
                    Address
                  </TableCell>
                  <TableCell align='center' sx={{ fontSize: '18px' }}>
                    Total price
                  </TableCell>
                  <TableCell align='center' sx={{ fontSize: '18px' }}>
                    Time placed
                  </TableCell>
                  {userRole === 'admin' && (
                    <TableCell align='center' sx={{ fontSize: '18px' }}>
                      Status
                    </TableCell>
                  )}
                  {userRole !== 'admin' && (
                    <TableCell align='center' sx={{ fontSize: '18px' }}>
                      Remaining time
                    </TableCell>
                  )}
                  {hasButton && (
                    <TableCell
                      align='center'
                      sx={{ fontSize: '18px' }}
                      colspan={
                        userRole === 'customer' && cancelOrderCallback ? 2 : 1
                      }
                    >
                      Action
                    </TableCell>
                  )}
                </TableRow>
              </TableHead>
              <TableBody>
                {data.map((row) => (
                  <TableRow
                    key={row.id}
                    sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                  >
                    <TableCell component='th' align='center' scope='row'>
                      {row.id}
                    </TableCell>
                    <TableCell align='center'>{row.comment}</TableCell>
                    <TableCell align='center'>{row.address}</TableCell>
                    <TableCell align='center'>{row.totalPrice}</TableCell>
                    <TableCell align='center'>
                      {getDateString(row.placedTime)}
                    </TableCell>
                    {userRole === 'admin' && (
                      <TableCell align='center'>
                        {row.remainingTime === '00:00:00'
                          ? 'Finished'
                          : 'In progress'}
                      </TableCell>
                    )}
                    {userRole !== 'admin' && (
                      <TableCell align='center'>{row.remainingTime}</TableCell>
                    )}
                    {hasButton && (
                      <TableCell align='center'>
                        <Button
                          variant='contained'
                          onClick={(e) => {
                            buttonCallback(row.id);
                          }}
                        >
                          {buttonText}
                        </Button>
                      </TableCell>
                    )}
                    {hasButton &&
                      userRole === 'customer' &&
                      cancelOrderCallback && (
                        <TableCell align='center'>
                          <Button
                            variant='contained'
                            color='secondary'
                            onClick={(e) => {
                              cancelOrderCallback(row.id);
                            }}
                          >
                            Cancel
                          </Button>
                        </TableCell>
                      )}
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </Container>
      )}
      {data.length === 0 && <NoData>No orders to show...</NoData>}
    </>
  );
};

export default Orders;
