import useDashboard from '../hooks/useDashboard';
import DashboardLayout from '../components/common/DashboardLayout';
import StatsGrid from '../components/common/StatsGrid';
import SearchBar from '../components/common/SearchBar';
import TransactionList from '../components/common/TransactionList';
import LoadingScreen from '../components/common/LoadingScreen';
import InfoCardMain from '../components/common/InfoCardMain';
import StandardExpenseList from '../components/common/StandardExpenseList';

export default function DashboardPage() {
  const {
    user,
    searchQuery,
    setSearchQuery,
    transactions,
    standardExpenses,
    categoryMap,
    currencyMap,
    walletMap,
    walletToCurrencyMap,
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

      <SearchBar 
        searchQuery={searchQuery}
        setSearchQuery={setSearchQuery}
        onSearch={handleSearch}
      />

      <StatsGrid stats={stats} />
      <div className="flex flex-col gap-6 mt-6">
      <TransactionList 
        transactions={transactions}
        categoryMap={categoryMap}
        currencyMap={currencyMap}
        walletMap={walletMap}
        searchQuery={searchQuery}
        onClearSearch={handleClearSearch}
      />

      <StandardExpenseList
        standardExpenses={standardExpenses}
        searchQuery={searchQuery}
        walletToCurrencyMap={walletToCurrencyMap}
        walletMap={walletMap}
        onClearSearch={handleClearSearch}
      />
      </div>
    </DashboardLayout>
  );
}