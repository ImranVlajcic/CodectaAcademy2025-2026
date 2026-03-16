import useTransactionsFilter from '../hooks/useTransactionsFilter';
import DashboardLayout from '../components/common/DashboardLayout';
import LoadingScreen from '../components/common/LoadingScreen';
import TransactionList from '../components/common/TransactionList';
import StandardExpenseList from '../components/common/StandardExpenseList';
import FilterPanel from '../components/common/FilterPanel';
import { Filter, X } from 'lucide-react';
import { useState } from 'react';

export default function TransactionsPage() {
  const {
    user,
    loading,
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
  } = useTransactionsFilter();

  const [showFilters, setShowFilters] = useState(true);

  if (loading) {
    return <LoadingScreen />;
  }

  return (
    <DashboardLayout user={user} onLogout={handleLogout}>
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-2xl font-bold text-gray-900">All Transactions</h1>
        <button
          onClick={() => setShowFilters(!showFilters)}
          className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
        >
          {showFilters ? <X className="w-5 h-5" /> : <Filter className="w-5 h-5" />}
          {showFilters ? 'Hide Filters' : 'Show Filters'}
        </button>
      </div>

      {showFilters && (
        <FilterPanel
          filters={filters}
          updateFilter={updateFilter}
          resetFilters={resetFilters}
          categories={categories}
        />
      )}

      <div className="card mt-6">
        <div className="flex items-center justify-between">
          <div>
            <p className="text-sm text-gray-600">
              Showing <span className="font-bold text-blue-600">{filteredTransactions.length}</span> transactions
              and <span className="font-bold text-purple-600">{filteredStandardExpenses.length}</span> recurring expenses
            </p>
          </div>
          {(filters.amountMin > 0 || filters.amountMax < 999999 || filters.categories.length > 0 || filters.transactionTypes.length > 0 || filters.showOnlyIncome || filters.showOnlyExpenses) && (
            <button
              onClick={resetFilters}
              className="text-sm text-red-600 hover:text-red-700 font-medium"
            >
              Clear all filters
            </button>
          )}
        </div>
      </div>

      <div className="flex flex-col gap-6 mt-6">
        <TransactionList 
          transactions={filteredTransactions}
          searchQuery=""
          categoryMap={categoryMap}
          currencyMap={currencyMap}
          walletMap={walletMap}
          onClearSearch={() => {}}
        />

        <StandardExpenseList
          standardExpenses={filteredStandardExpenses}
          searchQuery=""
          onClearSearch={() => {}}
          walletToCurrencyMap={walletToCurrencyMap}
          walletMap={walletMap}
        />
      </div>
    </DashboardLayout>
  );
}