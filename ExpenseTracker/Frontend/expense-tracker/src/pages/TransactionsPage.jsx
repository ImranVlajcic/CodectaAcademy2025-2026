import useDashboard from '../hooks/useDashboard';
import DashboardLayout from '../components/common/DashboardLayout';
import StatsGrid from '../components/common/StatsGrid';
import SearchBar from '../components/common/SearchBar';
import TransactionList from '../components/common/TransactionList';
import LoadingScreen from '../components/common/LoadingScreen';
import StandardExpenseList from '../components/common/StandardExpenseList';

export default function DashboardPage() {
  const {
    user,
    searchQuery,
    setSearchQuery,
    transactions,
    standardExpenses,
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
      <SearchBar 
        searchQuery={searchQuery}
        setSearchQuery={setSearchQuery}
        onSearch={handleSearch}
      />

      <StatsGrid stats={stats} />
      
      <div className="flex flex-col gap-6 mt-6">
                <TransactionList 
                transactions={transactions}
                searchQuery={searchQuery}
                onClearSearch={handleClearSearch}
              />
        
              <StandardExpenseList
                standardExpenses={standardExpenses}
                searchQuery={searchQuery}
                onClearSearch={handleClearSearch}
              />
              </div>
    </DashboardLayout>
  );
}