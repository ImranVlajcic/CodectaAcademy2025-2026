import { useState, useEffect, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from '../services/authService';
import transactionService from '../services/transactionService';
import toast from 'react-hot-toast';
import expenseService from '../services/standardexpenseservice';

export default function useListing() {
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

  const handleLogout = async () => {
    await authService.logout();
    toast.success('Logged out successfully');
    navigate('/login');
  }; 

  return {
    user,
    transactions: filteredTransactions,
    standardExpenses: filteredStandardExpenses,
    loading,
    handleLogout,
  };
}