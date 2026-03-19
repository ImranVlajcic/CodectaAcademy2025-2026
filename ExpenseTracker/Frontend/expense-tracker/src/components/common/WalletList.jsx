import WalletCard from './WalletsCard';

export default function WalletsList({ 
  wallets = [],
  currencyMap,
  onWalletDeleted, 
}) {
  return (
    <div>
      {wallets.length > 0 ? (
        <div className="space-y-3">
          {wallets.map((wallet) => (
            <WalletCard
              key={wallet.walletID} 
              wallet={wallet}
              onDelete={onWalletDeleted} 
              currencyCode={currencyMap[wallet.currencyID]}
            />
          ))}
        </div>
      ) : (
        <div className="text-center py-12 bg-gray-50 rounded-xl border-2 border-dashed mb-6">
          <p className="text-gray-500">No wallets found. Create one to get started!</p>
        </div>
      )}
    </div>
  );
}