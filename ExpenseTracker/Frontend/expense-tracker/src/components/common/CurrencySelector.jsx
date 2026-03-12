import { useState, useEffect } from 'react';
import currencyService from '../../services/currencyservice';

export default function CurrencySelector({value, onChange}){
    const [currencies, setCurrencies] = useState([]);

    useEffect(() => {
    const fetchCurrency = async () => {
      try {
        const response = await currencyService.getAll();
        console.log('API Response:', response);
        
        const data = response.currencies || response || [];
        setCurrencies(data);
      } catch (err) {
        console.error('Failed to fetch currencies:', err);
      }
    };

    fetchCurrency();
  }, []);

    return (
        <select
      className="w-96 h-10 bg-gray-200 border-gray-100"
      value={value}
      onChange={(e) => onChange(Number(e.target.value))}
    >
      <option value="">Select currency</option>

      {currencies.map((currency) => (
        <option key={currency.currencyID} value={currency.currencyID}>
          {currency.currencyCode}
        </option>
      ))}
    </select>
    )
}