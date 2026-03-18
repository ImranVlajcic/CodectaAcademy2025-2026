import useListing from '../hooks/useListing';
import DashboardLayout from '../components/common/DashboardLayout';
import TransactionList from '../components/common/TransactionList';
import LoadingScreen from '../components/common/LoadingScreen';
import StandardExpenseList from '../components/common/StandardExpenseList';
import Button from '../components/common/Button';
import { Plus, CheckSquare, Square } from 'lucide-react';
import { useNavigate, useLocation } from 'react-router-dom';
import { useState } from 'react';

export default function ListingPage() {
  const {
    user,
    transactions,
    standardExpenses,
    categoryMap,
    currencyMap,
    walletMap,
    walletToCurrencyMap,
    loading,
    handleLogout,
    handleDeleteTransactions,
    handleDeleteExpenses,
    refreshData,
  } = useListing();

  const navigate = useNavigate();
  const { state } = useLocation();              
  const walletId = state?.walletId

  const [selectionMode, setSelectionMode] = useState(false);

  if (loading) {
    return <LoadingScreen />;
  }

  return (
    <DashboardLayout user={user} onLogout={handleLogout}>
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-2">
          <Button
            type="submit"
            variant="add"
            loading={loading}
            icon={Plus}
            onClick={() => navigate('/wallets/add-standard-expense', { state: { walletId } })}
          >
            Add Standard Expense
          </Button>
          <Button
            type="submit"
            variant="add"
            loading={loading}
            icon={Plus}
            onClick={() => navigate('/wallets/add-transaction', { state: { walletId } })}
          >
            Add Transaction
          </Button>
          <Button
          type="button"
          variant={selectionMode ? "danger" : "secondary"}
          icon={selectionMode ? CheckSquare : Square}
          onClick={() => setSelectionMode(!selectionMode)}
        >
          {selectionMode ? 'Cancel Selection' : 'Select Items'}
        </Button>
      </div>

      {selectionMode && (
        <div className="mb-6 p-4 bg-blue-50 border border-blue-200 rounded-lg">
          <p className="text-sm text-blue-800 font-medium">
            ✓ Selection mode active - Check items to delete them
          </p>
        </div>
      )}

      <div className="flex flex-col gap-6 mt-6">
              <TransactionList 
              transactions={transactions}
              categoryMap={categoryMap}
              currencyMap={currencyMap}
              walletMap={walletMap}
              selectionMode={selectionMode}
              onDelete={handleDeleteTransactions}
            />
      
            <StandardExpenseList
              standardExpenses={standardExpenses}
              walletToCurrencyMap={walletToCurrencyMap}
              walletMap={walletMap}
              selectionMode={selectionMode}
              onDelete={handleDeleteExpenses}
            />
            </div>
    </DashboardLayout>
  );
}