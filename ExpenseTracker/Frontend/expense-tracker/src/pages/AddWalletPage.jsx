import DashboardLayout from '../components/common/DashboardLayout';
import useWallets from '../hooks/useWallets';
import WalletForm from '../components/common/WalletForm';

export default function AddWalletPage() {
  const {
    user,
    handleLogout,
  } = useWallets();

  return (
    <DashboardLayout user={user} onLogout={handleLogout}>
        <div className="flex flex-col items-center justify-center min-h-[70vh] w-full">
        <div className="w-full max-w-md p-8 bg-white rounded-2xl shadow-sm border border-gray-100">
          <h2 className="text-2xl font-bold text-gray-900 mb-6 text-center">Create New Wallet</h2>
          <WalletForm />
        </div>
      </div>
    </DashboardLayout>
  );
}