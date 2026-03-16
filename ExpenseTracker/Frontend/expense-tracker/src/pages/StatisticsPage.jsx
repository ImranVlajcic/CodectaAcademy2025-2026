import useStatistics from '../hooks/useStatistics';
import DashboardLayout from '../components/common/DashboardLayout';
import LoadingScreen from '../components/common/LoadingScreen';
import InfoCardMain from '../components/common/InfoCardMain';
import MonthlyBarChart from '../components/common/MonthlyBarChart';
import CategoryPieChart from '../components/common/CategoryPieChart';
import { Calendar } from 'lucide-react';

export default function StatisticsPage() {
  const {
    user,
    loading,
    overallStats,
    monthlyData,
    categoryData,
    selectedMonth,
    setSelectedMonth,
    handleLogout,
  } = useStatistics();

  if (loading) {
    return <LoadingScreen />;
  }

  return (
    <DashboardLayout user={user} onLogout={handleLogout}>
      <InfoCardMain overallStats={overallStats} />

      <div className="card mt-6">
        <div className="flex items-center justify-between flex-wrap gap-4">
          <div className="flex items-center gap-2">
            <Calendar className="w-5 h-5 text-blue-600" />
            <h2 className="text-lg font-bold text-gray-900">Select Month</h2>
          </div>
          <input
            type="month"
            value={selectedMonth}
            onChange={(e) => setSelectedMonth(e.target.value)}
            className="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none"
            max={new Date().toISOString().slice(0, 7)}
          />
        </div>
      </div>

      <div className="grid grid-cols-1 xl:grid-cols-2 gap-6 mt-6">
        <MonthlyBarChart data={monthlyData} selectedMonth={selectedMonth} />

        <CategoryPieChart data={categoryData} />
      </div>
    </DashboardLayout>
  );
}