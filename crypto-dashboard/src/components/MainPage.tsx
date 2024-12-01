import { ChangeEvent, useEffect, useState } from 'react';
import {
  AppBar,
  Toolbar,
  Typography,
  Button,
  Box,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  Pagination,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
} from '@mui/material';
import { useAppDispatch, useAppSelector } from '../hooks';
import { setToken, setUserInfo } from '../store/authSlice';
import { setCryptos, setTotalCount, setSearch, setCurrentPage } from '../store/cryptoSlice';
import container from '../container';
import AuthService from '../services/AuthService';
import CryptoService from '../services/CryptoService';
import debounce from 'lodash/debounce';

const MainPage = () => {
  const authService = container.get(AuthService);
  const cryptoService = container.get(CryptoService);

  const { token, userInfo } = useAppSelector((state) => state.auth);
  const { cryptos, totalCount, search, currentPage } = useAppSelector((state) => state.crypto);
  const dispatch = useAppDispatch();

  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [selectedCryptoId, setSelectedCryptoId] = useState<string | null>(null);
  const [selectedCryptoName, setSelectedCryptoName] = useState<string>('');
  const [itemsPerPage, setItemsPerPage] = useState(10);

  const fetchCryptos = async () => {
    if (!token) return;

    try {
      const count = await cryptoService.getCryptosCount(search, token);
      dispatch(setTotalCount(count));

      // If the current page becomes invalid, navigate to the previous page
      if (currentPage > Math.ceil(count / itemsPerPage)) {
        dispatch(setCurrentPage(Math.max(1, currentPage - 1)));
      }

      const cryptosData = await cryptoService.getCryptos(search, currentPage, itemsPerPage, token);
      dispatch(setCryptos(cryptosData));
    } catch (err) {
      console.error(err);
    }
  };

  const handleLogout = async () => {
    if (!token) return;
    try {
      await authService.logout(token);
      authService.clearToken();
      dispatch(setToken(null));
      dispatch(setUserInfo(null));
    } catch (err) {
      console.error('Error during logout:', err);
    }
  };

  const handleDeleteCrypto = async () => {
    if (!selectedCryptoId || !token) return;

    try {
      await cryptoService.removeCrypto(selectedCryptoId, token);
      setDeleteDialogOpen(false);
      setSelectedCryptoId(null);
      setSelectedCryptoName('');
      fetchCryptos(); // Refresh data after deletion
    } catch (err) {
      console.error(err);
    }
  };

  const debouncedSearch = debounce((value: string) => {
    dispatch(setSearch(value));
  }, 500);

  const handleSearchChange = (e: ChangeEvent<HTMLInputElement>) => {
    debouncedSearch(e.target.value);
  };

  useEffect(() => {
    fetchCryptos();
  }, [search, currentPage, itemsPerPage]);

  return (
    <Box>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" style={{ flexGrow: 1 }}>
            Logged as: <b>{userInfo?.username}</b>
          </Typography>
          <Button color="inherit" onClick={handleLogout}>
            Logout
          </Button>
        </Toolbar>
      </AppBar>

      <Box padding={2}>
        <TextField
          size="small"
          label="Search Cryptos"
          fullWidth
          onChange={handleSearchChange}
          style={{ marginBottom: '16px' }}
        />

        <Table size="small">
          <TableHead style={{ backgroundColor: '#f0f0f0' }}>
            <TableRow>
              <TableCell style={{ fontWeight: 'bold' }}>Cryptocurrency Name</TableCell>
              <TableCell style={{ fontWeight: 'bold' }}>Current Price (USD)</TableCell>
              <TableCell style={{ fontWeight: 'bold' }}>Market Capitalization</TableCell>
              <TableCell style={{ fontWeight: 'bold' }}>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {cryptos.length > 0 ? (
              cryptos.map((crypto, index) => (
                <TableRow
                  key={crypto.id}
                  style={{ backgroundColor: index % 2 === 0 ? '#f9f9f9' : '#ffffff' }}
                >
                  <TableCell>{crypto.name}</TableCell>
                  <TableCell>{crypto.currentPrice}</TableCell>
                  <TableCell>{crypto.marketCap}</TableCell>
                  <TableCell>
                    {userInfo?.roles.includes('admin') ? (
                      <Button
                        color="secondary"
                        onClick={() => {
                          setSelectedCryptoId(crypto.id);
                          setSelectedCryptoName(crypto.name);
                          setDeleteDialogOpen(true);
                        }}
                      >
                        Delete
                      </Button>
                    ) : (
                      <em>none</em>
                    )}
                  </TableCell>
                </TableRow>
              ))
            ) : (
              <TableRow>
                <TableCell colSpan={4} align="center">
                  <em>No items found.</em>
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>

        <Box
          display="flex"
          alignItems="center"
          justifyContent="flex-end"
          marginTop="16px"
        >
          <Pagination
            count={Math.ceil(totalCount / itemsPerPage)}
            page={currentPage}
            onChange={(e, value) => dispatch(setCurrentPage(value))}
          />
          <FormControl sx={{ m: 1, minWidth: 150 }}>
            <InputLabel id="items-per-page-label">Items per page</InputLabel>
            <Select
              size="small"
              labelId="items-per-page-label"
              value={itemsPerPage}
              label="Items per page"
              onChange={(e) => setItemsPerPage(e.target.value as number)}
            >
              <MenuItem value={10}>10</MenuItem>
              <MenuItem value={25}>25</MenuItem>
              <MenuItem value={50}>50</MenuItem>
            </Select>
          </FormControl>
        </Box>
      </Box>

      <Dialog open={deleteDialogOpen} onClose={() => setDeleteDialogOpen(false)}>
        <DialogTitle>Confirm Deletion</DialogTitle>
        <DialogContent>Are you sure you want to delete item '{selectedCryptoName}'?</DialogContent>
        <DialogActions>
          <Button onClick={() => setDeleteDialogOpen(false)}>Cancel</Button>
          <Button color="secondary" onClick={handleDeleteCrypto}>
            Delete
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default MainPage;
