import { useState, useEffect, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from '../services/authService';
import transactionService from '../services/transactionservice';
import standardExpenseService from '../services/standardexpenseservice';
import toast from 'react-hot-toast';

export default function useStatistics() {
  const navigate = useNavigate();
  const [user, setUser] = useState(null);
  const [transactions, setTransactions] = useState([]);
  const [standardExpenses, setStandardExpenses] = useState([]);
  const [loading, setLoading] = useState(true);
  
  const currentMonth = new Date().toISOString().slice(0, 7); 
  const [selectedMonth, setSelectedMonth] = useState(currentMonth);

  useEffect(() => {
    const currentUser = authService.getCurrentUser();
    setUser(currentUser);

    const fetchData = async () => {
      try {
        setLoading(true);
        
        const transactionsResponse = await transactionService.getAllByUser();
        const transactionsData = transactionsResponse.transactions || [];
        setTransactions(transactionsData);

        const expensesResponse = await standardExpenseService.getAllByUser();
        const expensesData = expensesResponse.standardExpenses || [];
        setStandardExpenses(expensesData);

      } catch (err) {
        console.error('Failed to fetch data:', err);
        toast.error('Failed to load statistics');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  const overallStats = useMemo(() => {
    return transactions.reduce((acc, t) => {
      const amount = parseFloat(t.amount) || 0;
      if (amount > 0) acc.income += amount;
      else acc.expense += Math.abs(amount);
      acc.total = acc.income - acc.expense;
      return acc;
    }, { income: 0, expense: 0, total: 0 });
  }, [transactions]);

  const monthlyTransactions = useMemo(() => {
    return transactions.filter(t => {
      const transactionMonth = t.transactionDate?.slice(0, 7);
      return transactionMonth === selectedMonth;
    });
  }, [transactions, selectedMonth]);

  const monthlyData = useMemo(() => {
    const daysInMonth = new Date(
      parseInt(selectedMonth.split('-')[0]),
      parseInt(selectedMonth.split('-')[1]),
      0
    ).getDate();

    const dailyData = {};
    for (let day = 1; day <= daysInMonth; day++) {
      dailyData[day] = { day, income: 0, expense: 0, standardExpense: 0 };
    }

    monthlyTransactions.forEach(t => {
      const day = parseInt(t.transactionDate?.split('-')[2] || 0);
      if (day > 0 && day <= daysInMonth) {
        const amount = parseFloat(t.amount) || 0;
        if (amount > 0) {
          dailyData[day].income += amount;
        } else {
          dailyData[day].expense += Math.abs(amount);
        }
      }
    });

    standardExpenses.forEach(se => {
      const nextDate = new Date(se.nextDate);
      const expenseMonth = nextDate.toISOString().slice(0, 7);
      
      if (expenseMonth === selectedMonth) {
        const day = nextDate.getDate();
        if (day > 0 && day <= daysInMonth) {
          dailyData[day].standardExpense += Math.abs(parseFloat(se.amount) || 0);
        }
      }
    });

    return Object.values(dailyData);
  }, [monthlyTransactions, standardExpenses, selectedMonth]);

  const categoryData = useMemo(() => {
    const categoryTotals = {};

    monthlyTransactions.forEach(t => {
      const categoryId = t.categoryID;
      const amount = Math.abs(parseFloat(t.amount) || 0);
      
      const categoryName = `Category ${categoryId}`;
      
      if (!categoryTotals[categoryName]) {
        categoryTotals[categoryName] = 0;
      }
      categoryTotals[categoryName] += amount;
    });

    return Object.entries(categoryTotals).map(([name, value]) => ({
      name,
      value: parseFloat(value.toFixed(2))
    }));
  }, [monthlyTransactions]);

  const handleLogout = async () => {
    await authService.logout();
    toast.success('Logged out successfully');
    navigate('/login');
  };

  return {
    user,
    loading,
    overallStats,
    monthlyData,
    categoryData,
    selectedMonth,
    setSelectedMonth,
    handleLogout,
  };
}