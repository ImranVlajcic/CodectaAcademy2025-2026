import DashboardLayout from '../components/common/DashboardLayout';
import useStandardExpenses from '../hooks/useStandardExpenses';
import StandardExpenseForm from '../components/common/StandardExpenseForm';

export default function AddStandardExpensePage() {
  const {
    user,
    handleLogout,
  } = useStandardExpenses();

  return (
    <DashboardLayout user={user} onLogout={handleLogout}>
      <div className="flex flex-col items-center justify-center min-h-[70vh] w-full">
        <div className="w-full max-w-md p-8 bg-white rounded-2xl shadow-sm border border-gray-100">
          <h2 className="text-2xl font-bold text-gray-900 mb-6 text-center">
            Add Recurring Expense
          </h2>
          <StandardExpenseForm />
        </div>
      </div>
    </DashboardLayout>
  );
}