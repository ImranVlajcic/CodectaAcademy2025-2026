import useDashboard from '../hooks/useDashboard';
import DashboardLayout from '../components/common/DashboardLayout';
import StatsGrid from '../components/common/StatsGrid';
import SearchBar from '../components/common/SearchBar';
import TransactionList from '../components/common/TransactionList';
import LoadingScreen from '../components/common/LoadingScreen';
import InfoCardMain from '../components/common/InfoCardMain';

export default function DashboardPage() {
  const {
    user,
    searchQuery,
    setSearchQuery,
    transactions,
    loading,
    stats,
    overallStats,
    handleSearch,
    handleClearSearch,
    handleLogout,
  } = useDashboard();

  if (loading) {
    return <LoadingScreen />;
  }

  return (
    <DashboardLayout user={user} onLogout={handleLogout}>
      <InfoCardMain
              overallStats={overallStats}
        />
      
    </DashboardLayout>
  );
}