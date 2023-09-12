import { Link as RouterLink } from 'react-router-dom';

import { Link, Container, Paper, Typography } from '@mui/material';

const NotFound = () => {
  return (
    <>
      {' '}
      <Container sx={{ display: 'flex', justifyContent: 'center' }}>
        <Paper
          sx={{
            width: '60%',
            display: 'flex',
            flexDirection: 'column',
            textAlign: 'center',
            marginTop: '12vh',
            padding: '20px',
          }}
          elevation={4}
        >
          <Typography
            variant='h3'
            color='primary'
            sx={{ marginBottom: '15px' }}
          >
            The page you are looking for has not been found!
          </Typography>
          <Typography variant='h4' color='primary'>
            Return&nbsp;
            <Link
              component={RouterLink}
              to='/'
              underline='hover'
              color='secondary'
            >
              home
            </Link>
            ...
          </Typography>
        </Paper>
      </Container>
    </>
  );
};

export default NotFound;
