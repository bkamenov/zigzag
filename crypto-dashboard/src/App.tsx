import { useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import LoginPage from './components/LoginPage';
import MainPage from './components/MainPage';
import { useAppDispatch, useAppSelector } from './hooks';
import AuthService from './services/AuthService';
import { setToken, setUserInfo } from './store/authSlice';

const authService = new AuthService();

function App() {
  const dispatch = useAppDispatch();
  const token = useAppSelector((state) => state.auth.token);

  useEffect(() => {
    const savedToken = authService.loadToken();

    if (savedToken) {
      authService
        .getLoginInfo(savedToken)
        .then((userInfo) => {
          dispatch(setToken(savedToken));
          dispatch(setUserInfo(userInfo));
        })
        .catch(() => authService.clearToken());
    }
  }, [dispatch]);

  return (
    <Router>
      <Routes>
        <Route path="/" element={!token ? <LoginPage /> : <Navigate to="/main" />} />
        <Route path="/main" element={token ? <MainPage /> : <Navigate to="/" />} />
      </Routes>
    </Router>
  );
}

export default App;
