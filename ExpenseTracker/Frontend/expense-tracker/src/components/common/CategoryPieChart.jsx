import { PieChart, Pie, Cell, ResponsiveContainer, Legend, Tooltip } from 'recharts';
import { PieChart as PieChartIcon } from 'lucide-react';

const COLORS = [
  '#3b82f6', 
  '#ef4444', 
  '#10b981', 
  '#f59e0b', 
  '#8b5cf6', 
  '#ec4899', 
  '#14b8a6', 
  '#f97316', 
  '#06b6d4', 
  '#6366f1', 
];

export default function CategoryPieChart({ data }) {

  const total = data.reduce((sum, entry) => sum + entry.value, 0);

  const dataWithPercentage = data.map(entry => ({
    ...entry,
    percentage: ((entry.value / total) * 100).toFixed(1)
  }));

  const renderCustomLabel = ({ cx, cy, midAngle, innerRadius, outerRadius, percent }) => {
    const RADIAN = Math.PI / 180;
    const radius = innerRadius + (outerRadius - innerRadius) * 0.5;
    const x = cx + radius * Math.cos(-midAngle * RADIAN);
    const y = cy + radius * Math.sin(-midAngle * RADIAN);

    if (percent < 0.05) return null;

    return (
      <text 
        x={x} 
        y={y} 
        fill="white" 
        textAnchor={x > cx ? 'start' : 'end'} 
        dominantBaseline="central"
        className="font-bold text-sm"
      >
        {`${(percent * 100).toFixed(0)}%`}
      </text>
    );
  };

  return (
    <div className="card">
      <div className="mb-6">
        <div className="flex items-center gap-2 mb-2">
          <PieChartIcon className="w-5 h-5 text-blue-600" />
          <h2 className="text-xl font-bold text-gray-900">Category Breakdown</h2>
        </div>
        <p className="text-sm text-gray-500">Spending distribution by category</p>
      </div>

      {data.length > 0 ? (
        <>

          <ResponsiveContainer width="100%" height={300}>
            <PieChart>
              <Pie
                data={dataWithPercentage}
                cx="50%"
                cy="50%"
                labelLine={false}
                label={renderCustomLabel}
                outerRadius={100}
                fill="#8884d8"
                dataKey="value"
              >
                {dataWithPercentage.map((entry, index) => (
                  <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                ))}
              </Pie>
              <Tooltip 
                contentStyle={{ 
                  backgroundColor: 'white', 
                  border: '1px solid #e5e7eb',
                  borderRadius: '8px',
                  boxShadow: '0 4px 6px -1px rgba(0, 0, 0, 0.1)'
                }}
                formatter={(value, name, props) => [
                  `${value.toFixed(2)} (${props.payload.percentage}%)`,
                  name
                ]}
              />
            </PieChart>
          </ResponsiveContainer>

          <div className="mt-6 space-y-2 max-h-60 overflow-y-auto">
            {dataWithPercentage.map((entry, index) => (
              <div 
                key={`legend-${index}`} 
                className="flex items-center justify-between p-3 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors"
              >
                <div className="flex items-center gap-3">
                  <div 
                    className="w-4 h-4 rounded-full" 
                    style={{ backgroundColor: COLORS[index % COLORS.length] }}
                  />
                  <span className="font-medium text-gray-900">{entry.name}</span>
                </div>
                <div className="text-right">
                  <p className="font-bold text-gray-900">{entry.value.toFixed(2)}</p>
                  <p className="text-xs text-gray-500">{entry.percentage}%</p>
                </div>
              </div>
            ))}
          </div>

          <div className="mt-4 pt-4 border-t border-gray-200">
            <div className="flex items-center justify-between">
              <span className="font-semibold text-gray-700">Total Spending</span>
              <span className="text-xl font-bold text-gray-900">${total.toFixed(2)}</span>
            </div>
          </div>
        </>
      ) : (
        <EmptyState />
      )}
    </div>
  );
}

function EmptyState() {
  return (
    <div className="text-center py-12">
      <div className="w-16 h-16 bg-gray-100 rounded-full flex items-center justify-center mx-auto mb-4">
        <PieChartIcon className="w-8 h-8 text-gray-400" />
      </div>
      <p className="text-gray-600 font-medium">No category data</p>
      <p className="text-sm text-gray-500 mt-1">
        Add some transactions to see category breakdown
      </p>
    </div>
  );
}