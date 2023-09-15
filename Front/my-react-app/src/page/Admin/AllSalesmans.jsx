import { useState, useEffect } from 'react';

import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import { Box, Button, Container, Typography } from '@mui/material';

import useServices from '../../services/useServices';
import { getDateString } from '../../helper/dateTimeUtils';

const AllSalesmans = () => {
  const {
    data,
    error,
    statusCode,
    isLoading,
    getAllSalesmansRequest,
    updateSalesmanStatusRequest,
    clearRequest,
  } = useServices();
  const [updatingStatus, setUpdatingStatus] = useState(false);
  const [fetchingSalesmans, setFetchingSalesmans] = useState(true);
  const [salesmans, setSalesmans] = useState([]);

  useEffect(() => {
    getAllSalesmansRequest();
  }, [getAllSalesmansRequest]);

  useEffect(() => {
    if (isLoading) {
      return;
    } else if (statusCode === 200 && !error && updatingStatus) {
      setUpdatingStatus(false);
      getAllSalesmansRequest();
      setFetchingSalesmans(true);
      clearRequest();
      alert('Successfully changed status!');
    } else if (statusCode === 200 && !error && data && fetchingSalesmans) {
      setFetchingSalesmans(false);
      data?.salesmans.forEach((salesman) => { console.log(salesman)
        salesman.salesmanProfileImage =
          'data:image/*;base64,' + salesman.salesmanProfileImage;
      });
      setSalesmans(data?.salesmans);
      clearRequest();
    } else if (statusCode !== 200 && error) {
      alert(statusCode, error);
    }
  }, [
    isLoading,
    statusCode,
    error,
    updatingStatus,
    data,
    fetchingSalesmans,
    getAllSalesmansRequest,
    clearRequest,
  ]);

  return (
    <>
      <Container
        sx={{
          margin: '0,60,0,0',
        }}
      >
        <TableContainer component={Paper} elevation={4} sx={{ marginTop: '100px'}}>
          <Table
            sx={{ padding: '5px', width: '100%', }}
            aria-label='simple table'
          >
            <TableHead>
              <TableRow>
                <TableCell align='center' sx={{ fontSize: '18px' }}>
                  Profile image
                </TableCell>
                <TableCell align='center' sx={{ fontSize: '18px' }}>
                  Username
                </TableCell>
                <TableCell align='center' sx={{ fontSize: '18px' }}>
                  Email
                </TableCell>
                <TableCell align='center' sx={{ fontSize: '18px' }}>
                  Firstname
                </TableCell>
                <TableCell align='center' sx={{ fontSize: '18px' }}>
                  Lastname
                </TableCell>
                <TableCell align='center' sx={{ fontSize: '18px' }}>
                  Address
                </TableCell>
                <TableCell align='center' sx={{ fontSize: '18px' }}>
                  Birthdate
                </TableCell>
                <TableCell align='center' sx={{ fontSize: '18px' }} colSpan={2}>
                  Status
                </TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {salesmans.map((row) => (
                <TableRow
                  key={row.username}
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
                        src={row.salesmanProfileImage}
                        alt=''
                        style={{ maxWidth: '120px', maxHeight: '120px' }}
                      />
                    </Box>
                  </TableCell>
                  <TableCell component='th' align='center' scope='row'>
                    {row.username}
                  </TableCell>
                  <TableCell align='center'>{row.email}</TableCell>
                  <TableCell align='center'>{row.firstname}</TableCell>
                  <TableCell align='center'>{row.lastname}</TableCell>
                  <TableCell align='center'>{row.address}</TableCell>
                  <TableCell align='center'>
                    {getDateString(row.dateOfBirth)}
                  </TableCell>
                  {row.approvalStatus > 0 && (
                    <TableCell align='center' colSpan={2}>
                      <Typography
                        color={
                          getStatusString(row.approvalStatus) === 'APPROVED'
                            ? 'primary'
                            : 'secondary'
                        }
                      >
                        {getStatusString(row.approvalStatus)}
                      </Typography>
                    </TableCell>
                  )}
                  {getStatusString(row.approvalStatus) === 'PENDING' && (
                    <TableCell align='center'>
                      <Button
                        variant='contained'
                        onClick={(e) => {
                          setUpdatingStatus(true);
                          updateSalesmanStatusRequest({
                            SalesmanStatus: true,
                            SalesmanName: row.username,
                          });
                        }}
                      >
                        Approve
                      </Button>
                    </TableCell>
                  )}
                  {getStatusString(row.approvalStatus) === 'PENDING' && (
                    <TableCell align='center'>
                      <Button
                        variant='contained'
                        color='secondary'
                        onClick={(e) => {
                          setUpdatingStatus(true);
                          updateSalesmanStatusRequest({
                            SalesmanStatus: false,
                            SalesmanName: row.username,
                          });
                        }}
                      >
                        Deny
                      </Button>
                    </TableCell>
                  )}
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      </Container>
    </>
  );
};

const getStatusString = (approval) => {
  switch (approval) {
    case 0:
      return 'PENDING';
    case 1:
      return 'APPROVED';
    case 2:
      return 'DENIED';
    default:
      return 'Unknown';
  }
};

export default AllSalesmans;
