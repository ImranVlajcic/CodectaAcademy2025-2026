import { useNavigate } from "react-router-dom"

export default function WalletCard({ wallet}){
    const navigate = useNavigate();

    const amountColor =
    (wallet.balance >= 0 ? 'text-green-600' : 'text-red-600')
    

    return<div className="stat-card gap-6 mb-8"
          onClick = {() => navigate('/wallets/listing', { state: { walletId: wallet.walletID } })}
        >
        <div className="flex items-center justify-between mb-4">
        <div className={`text-3xl h-12 font-bold flex items-center justify-center `}>
        <p>{wallet.purpose}</p>
        </div>
      </div>
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-2">
      <div>
      <p className={`text-3xl font-bold ${amountColor}`}>
        {wallet.balance.toFixed(2)}
      </p>
      <p className="text-sm text-gray-600 mt-1">Balance</p>
      </div>
      </div>
    </div>
}