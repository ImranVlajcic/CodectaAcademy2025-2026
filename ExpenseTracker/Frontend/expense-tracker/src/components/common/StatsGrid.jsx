import { TrendingUp, TrendingDown, DollarSign } from 'lucide-react';
import StatCard from './StatsCard';

export default function StatsGrid({ stats }) {
  return (
    <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
      <StatCard
        amount={stats.income}
        subtitle="Total earnings"
        icon={<TrendingUp className="w-6 h-6 text-green-600" />}
        iconBg="bg-green-100"
        badgeColor="text-green-600 bg-green-50"
      />
      
      <StatCard
        amount={stats.expense}
        subtitle="Total spending"
        icon={<TrendingDown className="w-6 h-6 text-red-600" />}
        iconBg="bg-red-100"
        badgeColor="text-red-600 bg-red-50"
      />
      
      <StatCard
        amount={stats.total}
        subtitle={`${stats.total >= 0 ? 'Positive' : 'Negative'} ratio`}
        icon={<DollarSign className="w-6 h-6 text-blue-600" />}
        iconBg="bg-blue-100"
        badgeColor="text-blue-600 bg-blue-50"
        isBalance={true}
      />
    </div>
  );
}