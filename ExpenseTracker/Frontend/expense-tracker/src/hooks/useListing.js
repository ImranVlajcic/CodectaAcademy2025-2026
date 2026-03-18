import { useState, useEffect, useMemo, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from '../services/authService';
import transactionService from '../services/transactionService';
import toast from 'react-hot-toast';
import expenseService from '../services/standardexpenseservice';
import categoryService from '../services/categoryService';
import currencyService from '../services/currencyservice';
import walletService from '../services/walletservice';

export default function useListing() {
  const navigate = useNavigate();
  const [transactions, setTransactions] = useState([]);
  const [standardExpenses, setExpenses] = useState([]);
  const [wallets, setWallets] = useState([]);
  const [categories, setCategories] = useState([]);
  const [currencies, setCurrencies] = useState([]);
  const [loading, setLoading] = useState(true);
  const [user, setUser] = useState(null);

  useEffect(() => {
    const currentUser = authService.getCurrentUser();
    setUser(currentUser);

    const fetchTransactions = async () => {
      try {
        const response = await transactionService.getAllByUser();
        console.log('API Response:', response);
        
        const data = response.transactions|| response || [];
        setTransactions(data);
        toast.success(`Loaded ${data.length} transactions`, {
        id: 'transaction-toast', 
        });
      } catch (err) {
        console.error('Failed to fetch transactions:', err);
        toast.error('Failed to load transactions');
      } finally {
        setLoading(false);
      }
    };

    const fetchExpenses = async () => {
      try {
        const response = await expenseService.getAllByUser();
        console.log('API Response:', response);
        
        const data = response.standardExpenses || response || [];
        setExpenses(data);
        toast.success(`Loaded ${data.length} standard expenses`, {
        id: 'expense-toast', 
        });
      } catch (err) {
        console.error('Failed to fetch standard expenses:', err);
        toast.error('Failed to load standard expenses');
      } finally {
        setLoading(false);
      }
    };

    const fetchWallets = async () => {
      try {
        const response = await walletService.getAllByUser();
        console.log('API Response:', response);
        
        const data = response.wallets || response || [];
        setWallets(data);
      } catch (err) {
        console.error('Failed to fetch wallets:', err);
      } finally {
        setLoading(false);
      }
    };

    const fetchCategories = async () => {
      try {
        const response = await categoryService.getAll();
        console.log('API Response:', response);
        
        const data = response.categories || response || [];
        setCategories(data);
      } catch (err) {
        console.error('Failed to fetch categories:', err);
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

    fetchTransactions();
    fetchExpenses();
    fetchWallets();
    fetchCategories();
    fetchCurrencies();
  }, []);

  const handleLogout = async () => {
    await authService.logout();
    toast.success('Logged out successfully');
    navigate('/login');
  }; 

  const handleDeleteTransactions = async (transactionIds) => {
    try {
      const deletePromises = transactionIds.map(id => 
        transactionService.delete(id)
      );
 
      await Promise.all(deletePromises);
 
      setTransactions(prev => 
        prev.filter(t => !transactionIds.includes(t.transactionID))
      );
 
      toast.success(`Successfully deleted ${transactionIds.length} transaction${transactionIds.length > 1 ? 's' : ''}`);
    } catch (error) {
      console.error('Failed to delete transactions:', error);
      toast.error('Failed to delete transactions');
      throw error;
    }
  };
 
  const handleDeleteExpenses = async (expenseIds) => {
    try {

      const deletePromises = expenseIds.map(id => 
        expenseService.delete(id)
      );
 
      await Promise.all(deletePromises);
 
      setExpenses(prev => 
        prev.filter(e => !expenseIds.includes(e.expenseID))
      );
 
      toast.success(`Successfully deleted ${expenseIds.length} expense${expenseIds.length > 1 ? 's' : ''}`);
    } catch (error) {
      console.error('Failed to delete expenses:', error);
      toast.error('Failed to delete expenses');
      throw error;
    }
  };

  const categoryMap = useMemo(() => {
  return categories.reduce((acc, cat) => {
    acc[cat.categoryID] = cat.categoryName;
    return acc;
  }, {});
}, [categories]);

const currencyMap = useMemo(() => {
  return currencies.reduce((acc, cur) => {
    acc[cur.currencyID] = cur.currencyCode; 
    return acc;
  }, {});
}, [currencies]);

const walletMap = useMemo(() => {
  return wallets.reduce((acc, cur) => {
    acc[cur.walletID] = cur.purpose; 
    return acc;
  }, {});
}, [wallets]);

const walletToCurrencyMap = useMemo(() => {
  return wallets.reduce((acc, wallet) => {
    acc[wallet.walletID] = currencyMap[wallet.currencyID];
    return acc;
  }, {});
}, [wallets, currencyMap]);

  return {
    user,
    transactions,
    standardExpenses,
    categoryMap,
    currencyMap,
    walletMap,
    walletToCurrencyMap,
    loading,
    handleLogout,
    handleDeleteTransactions,
    handleDeleteExpenses,
    refreshData: () =>{
        fetchTransactions(),
        fetchCategories(),
        fetchCurrencies(),
        fetchWallets(),
        fetchExpenses()
    },
  };
}