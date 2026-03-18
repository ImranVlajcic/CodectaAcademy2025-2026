import { useState, useEffect, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from '../services/authService';
import transactionService from '../services/transactionservice';
import standardExpenseService from '../services/standardexpenseservice';
import toast from 'react-hot-toast';
import walletService from '../services/walletservice';
import categoryService from '../services/categoryService';
import currencyService from '../services/currencyservice';

export default function useStatistics() {
  const navigate = useNavigate();
  const [user, setUser] = useState(null);
  const [transactions, setTransactions] = useState([]);
  const [standardExpenses, setStandardExpenses] = useState([]);
  const [wallets, setWallets] =useState([]);
  const [categories, setCategories] = useState([]);
  const [currencies, setCurrencies] = useState([]);
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

        const walletResponse = await walletService.getAllByUser();
        const walletData = walletResponse.wallets || []
        setWallets(walletData);

        const categoryResponse = await categoryService.getAll();
        const categoryData = categoryResponse.categories || []
        setCategories(categoryData);

        const currencyResponse = await currencyService.getAll();
        const currencyData = currencyResponse.currencies || []
        setCurrencies(currencyData);

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
  const transactionStats = transactions.reduce((acc, t) => {
    const currency = currencies.find(c => c.currencyID === t.currencyID);
    const rate = currency?.rateToEuro || 1;
    const amountInEuro = (parseFloat(t.amount) || 0) * rate;

    if (amountInEuro > 0) acc.income += amountInEuro;
    else acc.expense += Math.abs(amountInEuro);
    
    return acc;
  }, { income: 0, expense: 0 });

  const totalWalletBalanceInEuro = wallets.reduce((acc, wallet) => {
    const currency = currencies.find(c => c.currencyID === wallet.currencyID);
    const rate = currency?.rateToEuro || 1;
    return acc + ((parseFloat(wallet.balance) || 0) * rate);
  }, 0);

  return {
    income: transactionStats.income,
    expense: transactionStats.expense,
    total: totalWalletBalanceInEuro 
  };
}, [transactions, currencies, wallets]);

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

  const categoryMap = useMemo(() => {
  return categories.reduce((acc, cat) => {
    acc[cat.categoryID] = cat.categoryName;
    return acc;
  }, {});
}, [categories]);

  const categoryData = useMemo(() => {
    if (categories.length === 0 || monthlyTransactions.length === 0) return [];

    const categoryTotals = {};

    monthlyTransactions.forEach(t => {
      const categoryName = categoryMap[t.categoryID] || 'Uncategorized';
      const amount = Math.abs(parseFloat(t.amount) || 0);
      
      if (!categoryTotals[categoryName]) {
        categoryTotals[categoryName] = 0;
      }
      categoryTotals[categoryName] += amount;
    });

    return Object.entries(categoryTotals).map(([name, value]) => ({
      name,
      value: parseFloat(value.toFixed(2))
    }))
    .sort((a, b) => b.value - a.value);
  }, [monthlyTransactions, categoryMap, categories]);

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