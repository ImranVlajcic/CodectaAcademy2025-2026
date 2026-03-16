import { useState, useEffect, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from '../services/authService';
import transactionService from '../services/transactionservice';
import expenseService from '../services/standardexpenseservice';
import toast from 'react-hot-toast';
import walletService from '../services/walletservice';
import categoryService from '../services/categoryService';
import currencyService from '../services/currencyservice';

export default function useTransactionsFilter() {
  const navigate = useNavigate();
  const [user, setUser] = useState(null);
  const [transactions, setTransactions] = useState([]);
  const [standardExpenses, setExpenses] = useState([]);
  const [categories, setCategories] = useState([]);
  const [wallets, setWallets] = useState([]);
  const [currencies, setCurrencies] = useState([]);
  const [loading, setLoading] = useState(true);

  const [filters, setFilters] = useState({
    amountMin: 0,
    amountMax: 999999,
    showOnlyIncome: false,
    showOnlyExpenses: false,
    categories: [],
    transactionTypes: [], 
    frequencies: [], 
    dateFrom: '',
    dateTo: '',
  });

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

  const updateFilter = (key, value) => {
    setFilters(prev => ({ ...prev, [key]: value }));
  };

  const resetFilters = () => {
    setFilters({
      amountMin: 0,
      amountMax: 999999,
      showOnlyIncome: false,
      showOnlyExpenses: false,
      categories: [],
      transactionTypes: [],
      frequencies: [],
      dateFrom: '',
      dateTo: '',
    });
  };

  const filteredTransactions = useMemo(() => {
    return transactions.filter(transaction => {
      const amount = Math.abs(parseFloat(transaction.amount) || 0);
      const isIncome = parseFloat(transaction.amount) > 0;

      if (amount < filters.amountMin || amount > filters.amountMax) {
        return false;
      }

      if (filters.showOnlyIncome && !isIncome) return false;
      if (filters.showOnlyExpenses && isIncome) return false;

      if (filters.categories.length > 0 && !filters.categories.includes(transaction.categoryID)) {
        return false;
      }

      if (filters.transactionTypes.length > 0 && !filters.transactionTypes.includes(transaction.transactionType)) {
        return false;
      }

      if (filters.dateFrom && transaction.transactionDate < filters.dateFrom) {
        return false;
      }
      if (filters.dateTo && transaction.transactionDate > filters.dateTo) {
        return false;
      }

      return true;
    });
  }, [transactions, filters]);

  const filteredStandardExpenses = useMemo(() => {
    return standardExpenses.filter(expense => {
      const amount = Math.abs(parseFloat(expense.amount) || 0);

      if (amount < filters.amountMin || amount > filters.amountMax) {
        return false;
      }

      if (filters.showOnlyIncome) return false;

      if (filters.frequencies.length > 0 && !filters.frequencies.includes(expense.frequency)) {
        return false;
      }

      if (filters.dateFrom && expense.nextDate < filters.dateFrom) {
        return false;
      }
      if (filters.dateTo && expense.nextDate > filters.dateTo) {
        return false;
      }

      return true;
    });
  }, [standardExpenses, filters]);

  const handleLogout = async () => {
    await authService.logout();
    toast.success('Logged out successfully');
    navigate('/login');
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
    loading,
    transactions,
    standardExpenses,
    filteredTransactions,
    filteredStandardExpenses,
    filters,
    categoryMap,
    currencyMap,
    walletMap,
    walletToCurrencyMap,
    updateFilter,
    resetFilters,
    categories,
    handleLogout,
  };
}