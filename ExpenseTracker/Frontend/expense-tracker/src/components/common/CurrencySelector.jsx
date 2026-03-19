import { useState, useEffect } from 'react';
import currencyService from '../../services/currencyservice';

export default function CurrencySelector({value, onChange}){
    const [currencies, setCurrencies] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
    const fetchCurrency = async () => {
      try {
        const response = await currencyService.getAll();
        console.log('API Response:', response);
        
        const data = response.currencies || response || [];
        setCurrencies(data);
      } catch (err) {
        console.error('Failed to fetch currencies:', err);
      }finally {
        setLoading(false);
      }
    };

    fetchCurrency();
  }, []);

    return (
        <select
      className="w-full px-4 py-3 bg-gray-50 border border-gray-200 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all"
      value={value}
      onChange={(e) => onChange(Number(e.target.value))}
      disabled={loading}
    >
      <option value="">
        {loading ? 'Loading currencies...' : 'Select currency'}
      </option>

      {currencies.map((currency) => (
        <option key={currency.currencyID} value={currency.currencyID}>
          {currency.currencyCode}
        </option>
      ))}
    </select>
    )
}