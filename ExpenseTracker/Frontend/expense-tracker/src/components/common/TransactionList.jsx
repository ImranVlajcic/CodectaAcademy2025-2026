import { Search, ChevronDown, ChevronRight } from 'lucide-react';
import TransactionCard from './TransactionCard';
import { useState } from 'react';

export default function TransactionList({ 
  transactions, 
  searchQuery, 
  onClearSearch 
}) {
  const [isExpanded, setIsExpanded] = useState(true);

  return (
    <div className="card">
      <div className="flex items-center justify-between mb-6">
        <div 
          className="flex items-center gap-2 cursor-pointer select-none group" 
          onClick={() => setIsExpanded(!isExpanded)}
        >
          {isExpanded ? (
            <ChevronDown className="w-5 h-5 text-gray-400 group-hover:text-blue-600" />
          ) : (
            <ChevronRight className="w-5 h-5 text-gray-400 group-hover:text-blue-600" />
          )}
          
          <h2 className="text-xl font-bold text-gray-900">
            Recent Transactions
            <span className="ml-2 text-sm font-normal text-gray-500">
              ({transactions.length} results)
            </span>
          </h2>
        </div>

        {searchQuery && (
          <button
            onClick={onClearSearch}
            className="text-sm text-blue-600 hover:text-blue-700 font-medium"
          >
            Clear search
          </button>
        )}
      </div>

      {isExpanded && (
        <>
          {transactions.length > 0 ? (
            <div className="space-y-3 animate-in fade-in duration-300">
              {transactions.map((transaction) => (
                <TransactionCard 
                  key={transaction.transactionID} 
                  transaction={transaction} 
                />
              ))}
            </div>
          ) : (
            <EmptyState searchQuery={searchQuery} />
          )}
        </>
      )}
    </div>
  );
}

function EmptyState({ searchQuery }) {
  return (
    <div className="text-center py-12">
      <div className="w-16 h-16 bg-gray-100 rounded-full flex items-center justify-center mx-auto mb-4">
        <Search className="w-8 h-8 text-gray-400" />
      </div>
      <p className="text-gray-600 font-medium">No transactions found</p>
      <p className="text-sm text-gray-500 mt-1">
        {searchQuery 
          ? "Try a different search term" 
          : "Start by adding your first transaction"}
      </p>
    </div>
  );
}