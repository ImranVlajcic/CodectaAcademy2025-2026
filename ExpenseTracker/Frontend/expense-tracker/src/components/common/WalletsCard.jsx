import { useNavigate } from "react-router-dom"
import { Trash2 } from 'lucide-react';
import { useState } from 'react';
import DeleteWalletModal from '../common/DeleteWalletModal';
import walletService from '../../services/walletservice';
import toast from 'react-hot-toast';

export default function WalletCard({ wallet, onDelete}){
    const navigate = useNavigate();
    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [deleting, setDeleting] = useState(false);

    const amountColor =
    (wallet.balance >= 0 ? 'text-green-600' : 'text-red-600')

     const handleCardClick = () => {
    navigate('/wallets/listing', { state: { walletId: wallet.walletID } });
  };
    
    const handleDeleteClick = (e) => {
    e.stopPropagation(); 
    setShowDeleteModal(true);
  };
 
  const handleConfirmDelete = async () => {
    try {
      setDeleting(true);
      await walletService.delete(wallet.walletID);
      toast.success(`Wallet "${wallet.purpose}" deleted successfully`);
      setShowDeleteModal(false);
      
      if (onDelete) {
        onDelete(wallet.walletID);
      }
    } catch (err) {
      console.error('Failed to delete wallet:', err);
      toast.error('Failed to delete wallet');
    } finally {
      setDeleting(false);
    }
  };
 
  const handleCancelDelete = () => {
    setShowDeleteModal(false);
  };

    return (
    <>
      <div className="stat-card gap-6 mb-8 relative">
        <button
          onClick={handleDeleteClick}
          className="absolute top-4 right-4 p-2 bg-red-100 hover:bg-red-200 text-red-600 rounded-lg transition-colors z-10"
          title="Delete wallet"
        >
          <Trash2 className="w-5 h-5" />
        </button>
 
        <div 
          onClick={handleCardClick}
          className="cursor-pointer"
        >
          <div className="flex items-center justify-between mb-4">
            <div className="text-3xl h-12 font-bold flex items-center justify-center pr-12">
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
      </div>
 
      {showDeleteModal && (
        <DeleteWalletModal
          wallet={wallet}
          onConfirm={handleConfirmDelete}
          onCancel={handleCancelDelete}
          loading={deleting}
        />
      )}
    </>
  );
}