import { FormEvent, useState } from 'react';
import { useAppDispatch } from '../hooks';
import { setToken, setUserInfo } from '../store/authSlice';
import { TextField, Button, Box, Typography } from '@mui/material';
import AuthService from '../services/AuthService';
import container from '../container';



const LoginPage = () => {
  const authService = container.get(AuthService);

  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  const dispatch = useAppDispatch();

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault(); // Prevent page reload

    setError('');

    try {
      const token = await authService.login(username, password);
      authService.saveToken(token);
      dispatch(setToken(token));

      const userInfo = await authService.getLoginInfo(token);
      dispatch(setUserInfo(userInfo));
    } catch (err: any) {
      if (err.response?.status === 401) {
        setError('Invalid username or password.');
      } else {
        setError('An unexpected error occurred.');
      }
    }
  };

  return (
    <Box
      display="flex"
      flexDirection="column"
      alignItems="center"
      justifyContent="center"
      height="100vh"
    >
      <Box width={300}>
        <form onSubmit={handleSubmit}>
          <TextField
            fullWidth
            label="Username"
            autoComplete="username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            margin="normal"
            InputLabelProps={{
              shrink: true, // Always shrinks the label
            }}
          />
          <TextField
            fullWidth
            label="Password"
            type="password"
            autoComplete="current-password"
            name="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            margin="normal"
            InputLabelProps={{
              shrink: true, // Always shrinks the label
            }}
          />
          {error && (
            <Typography color="error" variant="body2" textAlign="center">
              {error}
            </Typography>
          )}
          <Button
            fullWidth
            variant="contained"
            type="submit"
            style={{ marginTop: '16px' }}
          >
            Login
          </Button>
        </form>
      </Box>
    </Box>
  );
};

export default LoginPage;
