import { useState, useEffect, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from '../services/authService';
import transactionService from '../services/transactionService';
import toast from 'react-hot-toast';
import expenseService from '../services/standardexpenseservice';

export default function useDashboard() {
  const navigate = useNavigate();
  const [searchQuery, setSearchQuery] = useState('');
  const [transactions, setTransactions] = useState([]);
  const [standardExpenses, setExpenses] = useState([]);
  const [filteredTransactions, setFilteredTransactions] = useState([]);
  const [filteredStandardExpenses, setFilteredExpenses] = useState([]);
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
        setFilteredTransactions(data);
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
        setFilteredExpenses(data);
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

    fetchTransactions();
    fetchExpenses();
  }, []);

  const handleSearch = (e) => {
    e.preventDefault();
    if (!searchQuery.trim()) {
      setFilteredTransactions(transactions);
      return;
    }

    const filtered = transactions.filter(t => 
      t.description?.toLowerCase().includes(searchQuery.toLowerCase()) ||
      t.transactionType?.toLowerCase().includes(searchQuery.toLowerCase())
    );
    setFilteredTransactions(filtered);
    toast.success(`Found ${filtered.length} results`);
  };

  const handleClearSearch = () => {
    setSearchQuery('');
    setFilteredTransactions(transactions);
  };

  const handleLogout = async () => {
    await authService.logout();
    toast.success('Logged out successfully');
    navigate('/login');
  }; 

  const overallStats = useMemo(() => {
  const now = new Date();
  const year = now.getFullYear();
  const month = now.getMonth();
  
  const daysInMonth = new Date(year, month + 1, 0).getDate();

  const getWeeklyMultiplier = () => {
    const extraDays = daysInMonth - 28; 
    return 4 + (extraDays / 7); 
  };

  const stats = transactions.reduce((acc, t) => {
    const amount = parseFloat(t.amount) || 0;
    if (amount > 0) acc.income += amount;
    else acc.expense += Math.abs(amount);
    return acc;
  }, { income: 0, expense: 0, total: 0 });

  standardExpenses.forEach(exp => {
    const amount = Math.abs(parseFloat(exp.amount)) || 0;
    let multiplier = 0;

    switch (exp.frequency) {
      case 'Daily':
        multiplier = daysInMonth;
        break;
      case 'Weekly':
        multiplier = getWeeklyMultiplier();
        break;
      case 'Monthly':
        multiplier = 1;
        break;
      case 'Yearly':
        multiplier = (month === 0) ? 1 : 0;
        break;
      default:
        multiplier = 0;
    }

    stats.expense += (amount * multiplier);
  });

  stats.total = stats.income - stats.expense;
  return stats;
}, [transactions, standardExpenses]);

  const stats = filteredTransactions.reduce((acc, t) => {
    const amount = parseFloat(t.amount) || 0;
    if (t.amount>0) {
      acc.income += amount;
    } else {
      acc.expense += Math.abs(amount);
    }
    acc.total = acc.income - acc.expense;
    return acc;
  }, { income: 0, expense: 0, total: 0 });

  return {
    user,
    searchQuery,
    setSearchQuery,
    transactions: filteredTransactions,
    standardExpenses: filteredStandardExpenses,
    loading,
    stats,
    overallStats,
    handleSearch,
    handleClearSearch,
    handleLogout,
  };
}