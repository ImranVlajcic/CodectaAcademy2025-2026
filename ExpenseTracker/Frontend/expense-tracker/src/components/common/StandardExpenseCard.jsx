import { TrendingUp, TrendingDown } from 'lucide-react';

export default function StandardExpenseCard({ standardExpense, onClick, currencyCode }) {
  const amount = parseFloat(standardExpense.amount) || 0;
  
  return (
    <div className="transaction-card"
    onClick={() => onClick(standardExpense)}
    >
      <div className="flex items-center gap-4">
        


        <div>
          <p className="font-semibold text-gray-900 group-hover:text-blue-600 transition-colors">
            {standardExpense.reason || 'No description'}
          </p>
          <div className="flex items-center gap-3 mt-1">
            <span className="text-sm text-gray-500">
              {new Date(standardExpense.nextDate).toLocaleDateString()}
            </span>
            
            

          </div>
        </div>
      </div>
      <div className="text-right">
        <p className={`text-lg font-bold text-red-600`}>
          {'-'}{Math.abs(amount).toFixed(2)}{'('}{currencyCode}{')'}
        </p>
        <p className="text-xs text-gray-500 mt-1">
          {standardExpense.frequency || 'N/A'}
        </p>
      </div>
    </div>
  );
}