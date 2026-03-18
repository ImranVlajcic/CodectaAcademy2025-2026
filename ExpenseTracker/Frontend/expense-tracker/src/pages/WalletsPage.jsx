import DashboardLayout from '../components/common/DashboardLayout';
import LoadingScreen from '../components/common/LoadingScreen';
import WalletsList from '../components/common/WalletList';
import useWallets from '../hooks/useWallets';
import Button from '../components/common/Button'
import { Plus } from 'lucide-react';
import { useNavigate } from 'react-router-dom';

export default function WalletsPage() {
  const {
    user,
    loading,
    wallets,
    handleLogout,
    handleWalletDeleted,
  } = useWallets();

  const navigate = useNavigate();

  if (loading) {
    return <LoadingScreen />;
  }

  return (
    <DashboardLayout user={user} onLogout={handleLogout}>

      <WalletsList
        wallets={wallets}
        onWalletDeleted={handleWalletDeleted}
      />
      <Button
        type="submit"
        variant="add"
        loading={loading}
        icon={Plus}
        onClick={() => navigate('/add-wallet')}
      >
        Add wallet
      </Button>
    </DashboardLayout>
  );
}