import { useState, useEffect, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from '../services/authService';
import toast from 'react-hot-toast';
import walletService from '../services/walletservice';

export default function useWallets() {
  const navigate = useNavigate();
  const [user, setUser] = useState(null);
  const [wallets, setWallets] = useState([]); 
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

    fetchWallets();
  }, []);

  const handleWalletDeleted = useCallback((deletedWalletId) => {
    // Optimistically remove from UI
    setWallets(prev => prev.filter(w => w.walletID !== deletedWalletId));
    
    // Optional: Refetch to ensure data consistency
    // fetchWallets();
  }, []);


  const handleLogout = async () => {
    await authService.logout();
    toast.success('Logged out successfully');
    navigate('/login');
  }; 

  return {
    user,
    wallets,
    loading,
    handleLogout,
    handleWalletDeleted,
  };
}