import { TrendingUp, TrendingDown } from 'lucide-react';

export default function TransactionCard({ transaction }) {
  const isIncome = transaction.amount > 0;
  const amount = parseFloat(transaction.amount) || 0;
  
  const formatTime = (timeString) => {
  if (!timeString) return '';
  const parts = timeString.split(':');
  return `${parts[0]}:${parts[1]}`;
  };
  
  return (
    <div className="transaction-card">
      <div className="flex items-center gap-4">
        <div className={`w-12 h-12 rounded-xl flex items-center justify-center ${
          isIncome ? 'bg-emerald-100' : 'bg-red-100'
        }`}>
          {isIncome ? (
            <TrendingUp className="w-6 h-6 text-emerald-600" />
          ) : (
            <TrendingDown className="w-6 h-6 text-red-600" />
          )}
        </div>
        <div>
          <p className="font-semibold text-gray-900 group-hover:text-blue-600 transition-colors">
            {transaction.description || 'No description'}
          </p>
          <div className="flex items-center gap-3 mt-1">
            <span className="text-sm text-gray-500">
              {new Date(transaction.transactionDate).toLocaleDateString()}
            </span>
            <span className={`text-xs font-medium px-2 py-0.5 rounded-full bg-gray-300 text-black-700`}>
              {transaction.transactionType}
            </span>
          </div>
        </div>
      </div>
      <div className="text-right">
        <p className={`text-lg font-bold ${
          isIncome ? 'text-green-600' : 'text-red-600'
        }`}>
          {isIncome ? '+' : '-'}${Math.abs(amount).toFixed(2)}
        </p>
        <p className="text-xs text-gray-500 mt-1">
          {formatTime(transaction.transactionTime) || 'N/A'}
        </p>
      </div>
    </div>
  );
}