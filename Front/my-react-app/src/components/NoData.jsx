import { Typography, Container, Paper } from '@mui/material';

const NoData = ({ children }) => {
  return (
    <Container sx={{ display: 'flex', justifyContent: 'center' }}>
      <Paper
        sx={{
          width: '60%',
          display: 'flex',
          flexDirection: 'column',
          textAlign: 'center',
          marginTop: '10vh',
          padding: '20px',
        }}
        elevation={4}
      >
        <Typography variant='h3' color='primary'>
          {children}
        </Typography>
      </Paper>
    </Container>
  );
};

export default NoData;
