import { DollarSign, Tag, CreditCard, TrendingUp, TrendingDown, Calendar } from 'lucide-react';

export default function FilterPanel({ filters, updateFilter, resetFilters, categories }) {
  const handleCategoryToggle = (categoryId) => {
    const newCategories = filters.categories.includes(categoryId)
      ? filters.categories.filter(id => id !== categoryId)
      : [...filters.categories, categoryId];
    updateFilter('categories', newCategories);
  };

  const handleTransactionTypeToggle = (type) => {
    const newTypes = filters.transactionTypes.includes(type)
      ? filters.transactionTypes.filter(t => t !== type)
      : [...filters.transactionTypes, type];
    updateFilter('transactionTypes', newTypes);
  };

  const handleFrequencyToggle = (frequency) => {
    const newFrequencies = filters.frequencies.includes(frequency)
      ? filters.frequencies.filter(f => f !== frequency)
      : [...filters.frequencies, frequency];
    updateFilter('frequencies', newFrequencies);
  };

  return (
    <div className="card">
      <div className="flex items-center justify-between mb-6">
        <h2 className="text-xl font-bold text-gray-900">Filter Options</h2>
        <button
          onClick={resetFilters}
          className="text-sm text-blue-600 hover:text-blue-700 font-medium"
        >
          Reset All
        </button>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
 
        <div className="space-y-4">
          <div className="flex items-center gap-2 mb-3">
            <DollarSign className="w-5 h-5 text-green-600" />
            <h3 className="font-semibold text-gray-900">Amount Range</h3>
          </div>

          <div className="space-y-3">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Minimum Amount: ${filters.amountMin}
              </label>
              <input
                type="range"
                min="0"
                max="10000"
                step="50"
                value={filters.amountMin}
                onChange={(e) => updateFilter('amountMin', parseFloat(e.target.value))}
                className="w-full h-2 bg-gray-200 rounded-lg appearance-none cursor-pointer accent-green-600"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Maximum Amount: ${filters.amountMax === 999999 ? '∞' : filters.amountMax}
              </label>
              <input
                type="range"
                min="0"
                max="10000"
                step="50"
                value={filters.amountMax === 999999 ? 10000 : filters.amountMax}
                onChange={(e) => {
                  const val = parseFloat(e.target.value);
                  updateFilter('amountMax', val === 10000 ? 999999 : val);
                }}
                className="w-full h-2 bg-gray-200 rounded-lg appearance-none cursor-pointer accent-green-600"
              />
            </div>

            <div className="flex items-center justify-between text-sm text-gray-600 pt-2">
              <span>Range: ${filters.amountMin} - ${filters.amountMax === 999999 ? '∞' : filters.amountMax}</span>
            </div>
          </div>
        </div>

        <div className="space-y-4">
          <div className="flex items-center gap-2 mb-3">
            <TrendingUp className="w-5 h-5 text-blue-600" />
            <h3 className="font-semibold text-gray-900">Transaction Direction</h3>
          </div>

          <div className="space-y-2">
            <label className="flex items-center gap-3 p-3 bg-emerald-50 border border-emerald-200 rounded-lg cursor-pointer hover:bg-emerald-100 transition-colors">
              <input
                type="checkbox"
                checked={filters.showOnlyIncome}
                onChange={(e) => {
                  updateFilter('showOnlyIncome', e.target.checked);
                  if (e.target.checked) updateFilter('showOnlyExpenses', false);
                }}
                className="w-5 h-5 text-emerald-600 rounded focus:ring-2 focus:ring-emerald-500"
              />
              <div className="flex items-center gap-2 flex-1">
                <TrendingUp className="w-4 h-4 text-emerald-600" />
                <span className="font-medium text-gray-900">Income Only</span>
              </div>
            </label>

            <label className="flex items-center gap-3 p-3 bg-red-50 border border-red-200 rounded-lg cursor-pointer hover:bg-red-100 transition-colors">
              <input
                type="checkbox"
                checked={filters.showOnlyExpenses}
                onChange={(e) => {
                  updateFilter('showOnlyExpenses', e.target.checked);
                  if (e.target.checked) updateFilter('showOnlyIncome', false);
                }}
                className="w-5 h-5 text-red-600 rounded focus:ring-2 focus:ring-red-500"
              />
              <div className="flex items-center gap-2 flex-1">
                <TrendingDown className="w-4 h-4 text-red-600" />
                <span className="font-medium text-gray-900">Expenses Only</span>
              </div>
            </label>
          </div>
        </div>

        <div className="space-y-4">
          <div className="flex items-center gap-2 mb-3">
            <CreditCard className="w-5 h-5 text-indigo-600" />
            <h3 className="font-semibold text-gray-900">Payment Method</h3>
          </div>

          <div className="flex gap-2">
            <button
              onClick={() => handleTransactionTypeToggle('Cash')}
              className={`flex-1 p-3 rounded-lg border-2 transition-all ${
                filters.transactionTypes.includes('Cash')
                  ? 'bg-indigo-100 border-indigo-600 text-indigo-900 font-semibold'
                  : 'bg-white border-gray-300 text-gray-700 hover:border-indigo-400'
              }`}
            >
              💵 Cash
            </button>
            <button
              onClick={() => handleTransactionTypeToggle('Card')}
              className={`flex-1 p-3 rounded-lg border-2 transition-all ${
                filters.transactionTypes.includes('Card')
                  ? 'bg-indigo-100 border-indigo-600 text-indigo-900 font-semibold'
                  : 'bg-white border-gray-300 text-gray-700 hover:border-indigo-400'
              }`}
            >
              💳 Card
            </button>
          </div>

          <p className="text-xs text-gray-500 mt-2">
            {filters.transactionTypes.length === 0 && 'Showing all payment methods'}
            {filters.transactionTypes.length === 1 && `Showing only ${filters.transactionTypes[0]}`}
            {filters.transactionTypes.length === 2 && 'Showing both Cash and Card'}
          </p>
        </div>

        <div className="space-y-4">
          <div className="flex items-center gap-2 mb-3">
            <Calendar className="w-5 h-5 text-purple-600" />
            <h3 className="font-semibold text-gray-900">Recurring Frequency</h3>
          </div>

          <div className="grid grid-cols-2 gap-2">
            {['Daily', 'Weekly', 'Monthly', 'Yearly'].map((freq) => (
              <button
                key={freq}
                onClick={() => handleFrequencyToggle(freq)}
                className={`p-3 rounded-lg border-2 transition-all text-sm ${
                  filters.frequencies.includes(freq)
                    ? 'bg-purple-100 border-purple-600 text-purple-900 font-semibold'
                    : 'bg-white border-gray-300 text-gray-700 hover:border-purple-400'
                }`}
              >
                {freq}
              </button>
            ))}
          </div>

          <p className="text-xs text-gray-500 mt-2">
            {filters.frequencies.length === 0 && 'Showing all frequencies'}
            {filters.frequencies.length > 0 && `Showing: ${filters.frequencies.join(', ')}`}
          </p>
        </div>

        <div className="space-y-4 lg:col-span-2">
          <div className="flex items-center gap-2 mb-3">
            <Calendar className="w-5 h-5 text-blue-600" />
            <h3 className="font-semibold text-gray-900">Date Range</h3>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                From Date
              </label>
              <input
                type="date"
                value={filters.dateFrom}
                onChange={(e) => updateFilter('dateFrom', e.target.value)}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                To Date
              </label>
              <input
                type="date"
                value={filters.dateTo}
                onChange={(e) => updateFilter('dateTo', e.target.value)}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              />
            </div>
          </div>
        </div>

        <div className="space-y-4 lg:col-span-2">
          <div className="flex items-center gap-2 mb-3">
            <Tag className="w-5 h-5 text-orange-600" />
            <h3 className="font-semibold text-gray-900">Categories</h3>
          </div>

          {categories.length > 0 ? (
            <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-6 gap-2">
              {categories.map((category) => (
                <button
                  key={category.categoryID}
                  onClick={() => handleCategoryToggle(category.categoryID)}
                  className={`p-3 rounded-lg border-2 transition-all text-sm ${
                    filters.categories.includes(category.categoryID)
                      ? 'bg-orange-100 border-orange-600 text-orange-900 font-semibold'
                      : 'bg-white border-gray-300 text-gray-700 hover:border-orange-400'
                  }`}
                >
                  <div className="text-center">
                    <p className="font-medium truncate">{category.categoryName}</p>
                  </div>
                </button>
              ))}
            </div>
          ) : (
            <p className="text-sm text-gray-500 italic">No categories available</p>
          )}

          <p className="text-xs text-gray-500 mt-2">
            {filters.categories.length === 0 && 'Showing all categories'}
            {filters.categories.length > 0 && `Selected ${filters.categories.length} categor${filters.categories.length === 1 ? 'y' : 'ies'}`}
          </p>
        </div>
      </div>
    </div>
  );
}