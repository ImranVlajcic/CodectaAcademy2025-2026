import useListing from '../hooks/useListing';
import DashboardLayout from '../components/common/DashboardLayout';
import TransactionList from '../components/common/TransactionList';
import LoadingScreen from '../components/common/LoadingScreen';
import StandardExpenseList from '../components/common/StandardExpenseList';
import Button from '../components/common/Button';
import { Plus } from 'lucide-react';

export default function ListingPage() {
  const {
    user,
    transactions,
    standardExpenses,
    loading,
    handleLogout,
  } = useListing();

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
            onClick={() => navigate('/add-wallet')}
          >
            Add Standard Expense
          </Button>
          <Button
            type="submit"
            variant="add"
            loading={loading}
            icon={Plus}
            onClick={() => navigate('/add-wallet')}
          >
            Add Transaction
          </Button>
      </div>
      <div className="flex flex-col gap-6 mt-6">
              <TransactionList 
              transactions={transactions}
            />
      
            <StandardExpenseList
              standardExpenses={standardExpenses}
            />
            </div>
    </DashboardLayout>
  );
}