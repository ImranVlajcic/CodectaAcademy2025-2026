export default function InfoCardMain({ overallStats}){

    const amountColor =
    (overallStats.total >= 0 ? 'text-green-600' : 'text-red-600')
    

    return<div className="stat-card gap-6 mb-8">
        <div className="flex items-center justify-between mb-4">
        <div className={`text-3xl w-74 h-12 font-bold flex items-center justify-center `}>
        <p>Account information</p>
        </div>
      </div>
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-2">
      <div>
      <p className={`text-3xl font-bold`}>
        {overallStats.income.toFixed(2)}
      </p>
      <p className="text-sm text-gray-600 mt-1">Total earnings</p>
      </div>
      <div>
      <p className={`text-3xl font-bold`}>
        {overallStats.expense.toFixed(2)}
      </p>
      <p className="text-sm text-gray-600 mt-1">Total spending</p>
      </div>
      <div>
      <p className={`text-3xl font-bold ${amountColor}`}>
        {overallStats.total.toFixed(2)}
      </p>
      <p className="text-sm text-gray-600 mt-1">Balance</p>
      </div>
      </div>
    </div>
}