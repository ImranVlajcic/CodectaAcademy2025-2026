import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import { TrendingUp, TrendingDown, Repeat } from 'lucide-react';

export default function MonthlyBarChart({ data, selectedMonth }) {
  const totals = data.reduce((acc, day) => ({
    income: acc.income + day.income,
    expense: acc.expense + day.expense,
    standardExpense: acc.standardExpense + day.standardExpense
  }), { income: 0, expense: 0, standardExpense: 0 });

  const monthName = selectedMonth ? new Date(selectedMonth + '-01').toLocaleDateString('en-US', { 
    month: 'long', 
    year: 'numeric' 
  }) : '';

  return (
    <div className="card">
      <div className="mb-6">
        <h2 className="text-xl font-bold text-gray-900 mb-1">Monthly Overview</h2>
        <p className="text-sm text-gray-500">
          {monthName ? `${monthName} - ` : ''}Daily income, expenses, and recurring charges
        </p>
      </div>

      <div className="grid grid-cols-3 gap-3 mb-6">
        <div className="p-3 bg-gradient-to-br from-emerald-50 to-green-50 rounded-lg border border-emerald-100">
          <div className="flex items-center gap-2 mb-1">
            <TrendingUp className="w-4 h-4 text-emerald-600" />
            <span className="text-xs text-emerald-700 font-semibold">Income</span>
          </div>
          <p className="text-lg font-bold text-emerald-900">
            ${totals.income.toFixed(2)}
          </p>
        </div>

        <div className="p-3 bg-gradient-to-br from-red-50 to-orange-50 rounded-lg border border-red-100">
          <div className="flex items-center gap-2 mb-1">
            <TrendingDown className="w-4 h-4 text-red-600" />
            <span className="text-xs text-red-700 font-semibold">Expenses</span>
          </div>
          <p className="text-lg font-bold text-red-900">
            ${totals.expense.toFixed(2)}
          </p>
        </div>

        <div className="p-3 bg-gradient-to-br from-purple-50 to-pink-50 rounded-lg border border-purple-100">
          <div className="flex items-center gap-2 mb-1">
            <Repeat className="w-4 h-4 text-purple-600" />
            <span className="text-xs text-purple-700 font-semibold">Recurring</span>
          </div>
          <p className="text-lg font-bold text-purple-900">
            ${totals.standardExpense.toFixed(2)}
          </p>
        </div>
      </div>

      <ResponsiveContainer width="100%" height={350}>
        <BarChart data={data} margin={{ top: 10, right: 10, left: -20, bottom: 0 }}>
          <CartesianGrid strokeDasharray="3 3" stroke="#e5e7eb" />
          <XAxis 
            dataKey="day" 
            tick={{ fontSize: 12 }}
            stroke="#9ca3af"
            label={{ value: 'Day of Month', position: 'insideBottom', offset: -5, style: { fontSize: 12, fill: '#6b7280' } }}
          />
          <YAxis 
            tick={{ fontSize: 12 }}
            stroke="#9ca3af"
            label={{ value: 'Amount ($)', angle: -90, position: 'insideLeft', style: { fontSize: 12, fill: '#6b7280' } }}
          />
          <Tooltip 
            contentStyle={{ 
              backgroundColor: 'white', 
              border: '1px solid #e5e7eb',
              borderRadius: '8px',
              boxShadow: '0 4px 6px -1px rgba(0, 0, 0, 0.1)'
            }}
            formatter={(value) => `$${value.toFixed(2)}`}
          />
          <Legend 
            wrapperStyle={{ paddingTop: '20px' }}
            iconType="circle"
          />
          <Bar 
            dataKey="income" 
            fill="#10b981" 
            name="Income" 
            radius={[4, 4, 0, 0]}
          />
          <Bar 
            dataKey="expense" 
            fill="#ef4444" 
            name="Expenses" 
            radius={[4, 4, 0, 0]}
          />
          <Bar 
            dataKey="standardExpense" 
            fill="#a855f7" 
            name="Recurring" 
            radius={[4, 4, 0, 0]}
          />
        </BarChart>
      </ResponsiveContainer>
    </div>
  );
}