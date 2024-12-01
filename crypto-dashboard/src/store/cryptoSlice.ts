import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface Crypto {
  id: string;
  name: string;
  currentPrice: number;
  marketCap: number;
}

interface CryptoState {
  cryptos: Crypto[];
  totalCount: number;
  search: string;
  currentPage: number;
}

const initialState: CryptoState = {
  cryptos: [],
  totalCount: 0,
  search: '',
  currentPage: 1,
};

const cryptoSlice = createSlice({
  name: 'crypto',
  initialState,
  reducers: {
    setCryptos(state, action: PayloadAction<Crypto[]>) {
      state.cryptos = action.payload;
    },
    setTotalCount(state, action: PayloadAction<number>) {
      state.totalCount = action.payload;
    },
    setSearch(state, action: PayloadAction<string>) {
      state.search = action.payload;
    },
    setCurrentPage(state, action: PayloadAction<number>) {
      state.currentPage = action.payload;
    },
  },
});

export const { setCryptos, setTotalCount, setSearch, setCurrentPage } = cryptoSlice.actions;
export default cryptoSlice.reducer;
