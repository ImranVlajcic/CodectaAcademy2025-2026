export default function StatCard({ title, amount, subtitle, icon, iconBg, badgeColor, isBalance }) {
  const amountColor = isBalance 
    ? (amount >= 0 ? 'text-green-600' : 'text-red-600')
    : 'text-gray-900';
  
  return (
    <div className="stat-card">
      <div className="flex items-center mb-2 gap-2">
        <div className={`w-10 h-10 rounded-xl flex items-center justify-center ${iconBg}`}>
          {icon}
        </div>
        <p className={`text-2xl font-bold ${amountColor}`}>
        {Math.abs(amount).toFixed(2)}
      </p>
      </div>
      <p className="text-sm text-gray-600 mt-1">{subtitle}</p>
    </div>
  );
}