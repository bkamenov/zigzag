import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface AuthState {
  token: string | null;
  userInfo: { username: string; roles: string[] } | null;
}

const initialState: AuthState = {
  token: null,
  userInfo: null,
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    setToken(state, action: PayloadAction<string | null>) {
      state.token = action.payload;
    },
    setUserInfo(state, action: PayloadAction<{ username: string; roles: string[] } | null>) {
      state.userInfo = action.payload;
    },
  },
});

export const { setToken, setUserInfo } = authSlice.actions;
export default authSlice.reducer;
