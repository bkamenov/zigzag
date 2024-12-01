import { configureStore } from '@reduxjs/toolkit';
import authReducer from './authSlice';
import cryptoReducer from './cryptoSlice';

export const store = configureStore({
  reducer: {
    auth: authReducer,
    crypto: cryptoReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
