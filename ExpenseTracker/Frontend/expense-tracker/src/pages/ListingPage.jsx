import useListing from '../hooks/useListing';
import DashboardLayout from '../components/common/DashboardLayout';
import TransactionList from '../components/common/TransactionList';
import LoadingScreen from '../components/common/LoadingScreen';
import StandardExpenseList from '../components/common/StandardExpenseList';
import Button from '../components/common/Button';
import { Plus } from 'lucide-react';
import { useNavigate, useLocation } from 'react-router-dom';

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
  } = useListing();

  const navigate = useNavigate();
  const { state } = useLocation();              
  const walletId = state?.walletId

  if (loading) {
    return <LoadingScreen />;
  }

  return (
    <DashboardLayout user={user} onLogout={handleLogout}>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-2">
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
      </div>
      <div className="flex flex-col gap-6 mt-6">
              <TransactionList 
              transactions={transactions}
              categoryMap={categoryMap}
              currencyMap={currencyMap}
              walletMap={walletMap}
            />
      
            <StandardExpenseList
              standardExpenses={standardExpenses}
              walletToCurrencyMap={walletToCurrencyMap}
              walletMap={walletMap}
            />
            </div>
    </DashboardLayout>
  );
}