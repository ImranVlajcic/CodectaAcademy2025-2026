import { useState, useEffect, useCallback, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from '../services/authService';
import toast from 'react-hot-toast';
import walletService from '../services/walletservice';
import currencyService from '../services/currencyservice';

export default function useWallets() {
  const navigate = useNavigate();
  const [user, setUser] = useState(null);
  const [wallets, setWallets] = useState([]);
  const [currencies, setCurrencies] = useState([]); 
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const currentUser = authService.getCurrentUser();
    setUser(currentUser);

    const fetchWallets = async () => {
      try {
        setLoading(true);
        const response = await walletService.getAllByUser();
        const data = response.wallets || (Array.isArray(response) ? response : []);
        setWallets(data || []);
        toast.success(`Loaded ${data.length} wallets`, {
        id: 'wallet-toast', 
        });
      } catch (err) {
        console.error('Failed to fetch wallets:', err);
        toast.error('Failed to load wallets');
      } finally {
        setLoading(false);
      }
    };

    const fetchCurrencies = async () => {
          try {
            const response = await currencyService.getAll();
            console.log('API Response:', response);
            
            const data = response.currencies || response || [];
            setCurrencies(data);
          } catch (err) {
            console.error('Failed to fetch currencies:', err);
          } finally {
            setLoading(false);
          }
        };

    fetchWallets();
    fetchCurrencies();
  }, []);

  const handleWalletDeleted = useCallback((deletedWalletId) => {
    setWallets(prev => prev.filter(w => w.walletID !== deletedWalletId));
  }, []);


  const handleLogout = async () => {
    await authService.logout();
    toast.success('Logged out successfully');
    navigate('/login');
  }; 

  const currencyMap = useMemo(() => {
    return currencies.reduce((acc, cur) => {
      acc[cur.currencyID] = cur.currencyCode; 
      return acc;
    }, {});
  }, [currencies]);

  return {
    user,
    wallets,
    currencyMap,
    loading,
    handleLogout,
    handleWalletDeleted,
  };
}