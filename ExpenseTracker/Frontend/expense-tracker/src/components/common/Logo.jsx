import { Wallet } from 'lucide-react';

export default function Logo({ 
  size = 'md',
  showText = false,
  variant = 'blue' 
}) {
  const sizes = {
    sm: 'w-10 h-10',
    md: 'w-16 h-16',
    lg: 'w-20 h-20',
  };

  const iconSizes = {
    sm: 'w-5 h-5',
    md: 'w-8 h-8',
    lg: 'w-10 h-10',
  };

  const variants = {
    emerald: 'from-emerald-500 to-teal-600 shadow-emerald-500/30',
    blue: 'from-blue-500 to-indigo-600 shadow-blue-500/30',
    purple: 'from-violet-500 to-purple-600 shadow-violet-500/30',
  };

  return (
    <div className="inline-flex items-center gap-3">
      <div className={`
        ${sizes[size]} 
        bg-gradient-to-br ${variants[variant]} 
        rounded-2xl 
        flex items-center justify-center 
        shadow-lg
      `}>
        <Wallet className={`${iconSizes[size]} text-white`} />
      </div>
      
      {showText && (
        <span className="text-2xl font-bold text-gray-900">
          Expense Tracker
        </span>
      )}
    </div>
  );
}