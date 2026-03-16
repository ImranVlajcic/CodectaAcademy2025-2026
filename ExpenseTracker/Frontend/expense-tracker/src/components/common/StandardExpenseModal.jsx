import { X, Calendar, CalendarCheck, Repeat } from 'lucide-react';

export default function StandardExpenseModal({ standardExpense, onClose, currencyCode, walletName }) {
  if (!standardExpense) return null;

  const amount = parseFloat(standardExpense.amount) || 0;

  const formatDate = (dateString) => {
    if (!dateString) return 'N/A';
    return new Date(dateString).toLocaleDateString('en-US', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  };

  return (
    <>
      <div 
        className="fixed inset-0 bg-gray bg-opacity-50 backdrop-blur-sm z-40 animate-in fade-in duration-200"
        onClick={onClose}
      />

      <div className="fixed inset-0 z-50 flex items-center justify-center p-4 animate-in zoom-in duration-200">
        <div 
          className="bg-white rounded-2xl shadow-2xl max-w-md w-full max-h-[90vh] overflow-y-auto"
          onClick={(e) => e.stopPropagation()}
        >
          <div className={`p-6 border-b border-gray-200 bg-gradient-to-r from-red-50 to-pink-50`}>
            <div className="flex items-start justify-between">
              <div className="flex items-center gap-4">
                <div className="w-14 h-14 rounded-xl flex items-center justify-center bg-red-100">
                  <Repeat className="w-8 h-8 text-red-600" />
                </div>
                <div>
                  <h2 className="text-2xl font-bold text-gray-900">
                    -{Math.abs(amount).toFixed(2)}{'('}{currencyCode}{')'}
                  </h2>
                  <p className="text-sm font-medium text-red-700">
                    Recurring Expense
                  </p>
                </div>
              </div>
              <button
                onClick={onClose}
                className="p-2 hover:bg-white/50 rounded-lg transition-colors"
              >
                <X className="w-6 h-6 text-gray-500" />
              </button>
            </div>
          </div>

          <div className="p-6 space-y-6">
            <div>
              <h3 className="text-sm font-semibold text-gray-500 uppercase tracking-wide mb-2">
                Reason
              </h3>
              <p className="text-lg font-bold text-gray-900">
                {standardExpense.reason || 'No reason provided'}
              </p>
            </div>

            {standardExpense.description && (
              <div>
                <h3 className="text-sm font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  Description
                </h3>
                <p className="text-gray-700">
                  {standardExpense.description}
                </p>
              </div>
            )}

            <div className="space-y-4">
              <h3 className="text-sm font-semibold text-gray-500 uppercase tracking-wide">
                Expense Details
              </h3>

              <div className="flex items-center gap-3 p-4 bg-gray-50 rounded-xl border border-purple-100">
                <div className="w-12 h-12 bg-blue-100 rounded-lg flex items-center justify-center text-2xl">
                  <CalendarCheck className="text-blue-600"/>
                </div>
                <div className="flex-1">
                  <p className="text-sm text-gray-500">Frequency</p>
                  <p className="font-bold text-lg text-gray-900">
                    {standardExpense.frequency || 'N/A'}
                  </p>
                </div>
              </div>

              <div className="flex items-center gap-3 p-4 bg-gray-50 rounded-xl border border-blue-100">
                <div className="w-12 h-12 bg-blue-100 rounded-lg flex items-center justify-center">
                  <Calendar className="w-6 h-6 text-blue-600" />
                </div>
                <div className="flex-1">
                  <p className="text-sm text-gray-500">Next Payment</p>
                  <p className="font-semibold text-gray-900">
                    {formatDate(standardExpense.nextDate)}
                  </p>
                </div>
              </div>

              <div className="p-4 bg-gray-50 rounded-xl border border-red-100">
                <div className="flex items-center justify-between mb-3">
                  <span className="text-sm text-gray-500">{standardExpense.frequency?.toUpperCase() }</span>
                  <span className="text-2xl font-bold text-red-600">
                    {Math.abs(amount).toFixed(2)}{'('}{currencyCode}{')'}
                  </span>
                </div>
                <div className="grid grid-cols-3 gap-2 text-center">
                  <div className="bg-gray-200 p-2 rounded-lg">
                    <p className="text-xs text-gray-500">Annual</p>
                    <p className="font-semibold text-gray-900 text-sm">
                      {(Math.abs(amount) * getAnnualMultiplier(standardExpense.frequency)).toFixed(0)}
                    </p>
                  </div>
                  <div className="bg-gray-200 p-2 rounded-lg">
                    <p className="text-xs text-gray-500">Monthly</p>
                    <p className="font-semibold text-gray-900 text-sm">
                      {(Math.abs(amount) * getMonthlyMultiplier(standardExpense.frequency)).toFixed(2)}
                    </p>
                  </div>
                  <div className="bg-gray-200 p-2 rounded-lg">
                    <p className="text-xs text-gray-500">Daily</p>
                    <p className="font-semibold text-gray-900 text-sm">
                      {(Math.abs(amount) * getDailyMultiplier(standardExpense.frequency)).toFixed(2)}
                    </p>
                  </div>
                </div>
              </div>

              <div className="flex items-center gap-3 p-4 bg-gray-50 rounded-xl border border-blue-100">
                <div className="w-12 h-12 bg-blue-100 rounded-lg flex items-center justify-center">
                  <Calendar className="w-6 h-6 text-blue-600" />
                </div>
                <div className="flex-1">
                  <p className="text-sm text-gray-500">Wallet Name</p>
                  <p className="font-semibold text-gray-900">
                    {walletName || 'N/A'}
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

function getDailyMultiplier(frequency) {
  switch(frequency?.toLowerCase()) {
    case 'daily': return 1;
    case 'weekly': return 1/7;
    case 'monthly': return 1/30;
    case 'yearly': return 1/365;
    default: return 0;
  }
}

function getMonthlyMultiplier(frequency) {
  switch(frequency?.toLowerCase()) {
    case 'daily': return 30;
    case 'weekly': return 4.33;
    case 'monthly': return 1;
    case 'yearly': return 1/12;
    default: return 0;
  }
}

function getAnnualMultiplier(frequency) {
  switch(frequency?.toLowerCase()) {
    case 'daily': return 365;
    case 'weekly': return 52;
    case 'monthly': return 12;
    case 'yearly': return 1;
    default: return 0;
  }
}