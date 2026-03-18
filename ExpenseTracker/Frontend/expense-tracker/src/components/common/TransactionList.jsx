import { Search, ChevronDown, ChevronRight, ChevronLeft, ChevronsRight, ChevronsLeft, Trash2 } from 'lucide-react';
import TransactionCard from './TransactionCard';
import { useState, useEffect} from 'react';
import TransactionModal from './TransactionModal';
import DeleteConfirmModal from './DeleteConfirmModal';

const ITEMS_PER_PAGE = 10;

export default function TransactionList({ 
  transactions,
  categoryMap,
  currencyMap,
  walletMap, 
  searchQuery, 
  onClearSearch,
  selectionMode = false,
  onDelete 
}) {
  const [isExpanded, setIsExpanded] = useState(true);
  const [selectedTransaction, setSelectedTransaction] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [selectedIds, setSelectedIds] = useState(new Set());
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [deleting, setDeleting] = useState(false)
 
  const handleTransactionClick = (transaction) => {
    if (selectionMode) {
      handleToggleSelection(transaction.transactionID);
    } else {
      setSelectedTransaction(transaction);
    }
  };
 
  const handleToggleSelection = (id) => {
    setSelectedIds(prev => {
      const newSet = new Set(prev);
      if (newSet.has(id)) {
        newSet.delete(id);
      } else {
        newSet.add(id);
      }
      return newSet;
    });
  };
 
  const handleSelectAll = () => {
    if (selectedIds.size === currentTransactions.length) {
      setSelectedIds(new Set());
    } else {
      setSelectedIds(new Set(currentTransactions.map(t => t.transactionID)));
    }
  };
 
  const handleCloseModal = () => {
    setSelectedTransaction(null);
  };

  const handleDeleteClick = () => {
    if (selectedIds.size > 0) {
      setShowDeleteModal(true);
    }
  };
 
  const handleConfirmDelete = async () => {
    setDeleting(true);
    try {
      await onDelete(Array.from(selectedIds));
      setSelectedIds(new Set());
      setShowDeleteModal(false);
    } catch (error) {
      console.error('Delete failed:', error);
    } finally {
      setDeleting(false);
    }
  };
  
  const totalPages = Math.ceil(transactions.length / ITEMS_PER_PAGE);
  const startIndex = (currentPage - 1) * ITEMS_PER_PAGE;
  const endIndex = startIndex + ITEMS_PER_PAGE;
  const currentTransactions = transactions.slice(startIndex, endIndex);
 
  useEffect(() => {
    setCurrentPage(1);
  }, [transactions.length]);
 
  const goToPage = (page) => {
    setCurrentPage(Math.max(1, Math.min(page, totalPages)));
  };
 
  const goToFirstPage = () => setCurrentPage(1);
  const goToLastPage = () => setCurrentPage(totalPages);
  const goToPrevPage = () => setCurrentPage(prev => Math.max(1, prev - 1));
  const goToNextPage = () => setCurrentPage(prev => Math.min(totalPages, prev + 1));

  return (
    <>
    <div className="card">
      <div className="flex items-center justify-between mb-6">
        <div 
          className="flex items-center gap-2 cursor-pointer select-none group" 
          onClick={() => setIsExpanded(!isExpanded)}
        >
          {isExpanded ? (
            <ChevronDown className="w-5 h-5 text-gray-400 group-hover:text-blue-600" />
          ) : (
            <ChevronRight className="w-5 h-5 text-gray-400 group-hover:text-blue-600" />
          )}
          
          <h2 className="text-xl font-bold text-gray-900">
            Recent Transactions
            <span className="ml-2 text-sm font-normal text-gray-500">
              ({transactions.length} results)
            </span>
          </h2>
        </div>

        <div className="flex items-center gap-3">
            {selectionMode && selectedIds.size > 0 && (
              <button
                onClick={handleDeleteClick}
                className="flex items-center gap-2 px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors font-medium"
              >
                <Trash2 className="w-4 h-4" />
                Delete ({selectedIds.size})
              </button>
            )}

        {searchQuery && (
          <button
            onClick={onClearSearch}
            className="text-sm text-blue-600 hover:text-blue-700 font-medium"
          >
            Clear search
          </button>
        )}
      </div>
      </div>

      {isExpanded && (
          <>
            {transactions.length > 0 ? (
              <>

              {selectionMode && (
                  <div className="mb-4 pb-4 border-b border-gray-200">
                    <label className="flex items-center gap-3 cursor-pointer group">
                      <input
                        type="checkbox"
                        checked={selectedIds.size === currentTransactions.length && currentTransactions.length > 0}
                        onChange={handleSelectAll}
                        className="w-5 h-5 rounded border-gray-300 text-blue-600 focus:ring-blue-500 cursor-pointer"
                      />
                      <span className="text-sm font-medium text-gray-700 group-hover:text-blue-600">
                        Select all on this page ({currentTransactions.length})
                      </span>
                    </label>
                  </div>
                )}

              <div className="space-y-3 animate-in fade-in duration-300">
                  {currentTransactions.map((transaction) => (
                    <div key={transaction.transactionID} className="relative">
                      {selectionMode && (
                        <div className="absolute left-4 top-1/2 -translate-y-1/2 z-10">
                          <input
                            type="checkbox"
                            checked={selectedIds.has(transaction.transactionID)}
                            onChange={(e) => {
                              e.stopPropagation();
                              handleToggleSelection(transaction.transactionID);
                            }}
                            className="w-5 h-5 rounded border-gray-300 text-blue-600 focus:ring-blue-500 cursor-pointer"
                            onClick={(e) => e.stopPropagation()}
                          />
                        </div>
                      )}
                      <div className={selectionMode ? 'pl-12' : ''}>
                        <TransactionCard 
                          transaction={transaction}
                          currencyCode={currencyMap[transaction.currencyID]}
                          onClick={handleTransactionClick}
                          isSelected={selectedIds.has(transaction.transactionID)}
                        />
                      </div>
                    </div>
                  ))}
                </div>

                {totalPages > 1 && (
                  <div className="mt-6 pt-4 border-t border-gray-200">
                    <div className="flex items-center justify-between">
                      <div className="text-sm text-gray-600">
                        Showing <span className="font-semibold">{startIndex + 1}</span> to{' '}
                        <span className="font-semibold">{Math.min(endIndex, transactions.length)}</span> of{' '}
                        <span className="font-semibold">{transactions.length}</span> transactions
                      </div>
 
                      <div className="flex items-center gap-2">
                        <button
                          onClick={goToFirstPage}
                          disabled={currentPage === 1}
                          className="p-2 rounded-lg border border-gray-300 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
                          title="First page"
                        >
                          <ChevronsLeft className="w-4 h-4 text-gray-600" />
                        </button>
                        
                        <button
                          onClick={goToPrevPage}
                          disabled={currentPage === 1}
                          className="p-2 rounded-lg border border-gray-300 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
                          title="Previous page"
                        >
                          <ChevronLeft className="w-4 h-4 text-gray-600" />
                        </button>
 
                        <div className="flex items-center gap-1">
                          {[...Array(totalPages)].map((_, index) => {
                            const pageNum = index + 1;
                            if (
                              pageNum === 1 ||
                              pageNum === totalPages ||
                              (pageNum >= currentPage - 1 && pageNum <= currentPage + 1)
                            ) {
                              return (
                                <button
                                  key={pageNum}
                                  onClick={() => goToPage(pageNum)}
                                  className={`min-w-[2rem] px-3 py-1 rounded-lg text-sm font-medium transition-colors ${
                                    currentPage === pageNum
                                      ? 'bg-blue-600 text-white'
                                      : 'bg-white border border-gray-300 text-gray-700 hover:bg-gray-50'
                                  }`}
                                >
                                  {pageNum}
                                </button>
                              );
                            } else if (
                              pageNum === currentPage - 2 ||
                              pageNum === currentPage + 2
                            ) {
                              return <span key={pageNum} className="text-gray-400">...</span>;
                            }
                            return null;
                          })}
                        </div>
 
                        <button
                          onClick={goToNextPage}
                          disabled={currentPage === totalPages}
                          className="p-2 rounded-lg border border-gray-300 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
                          title="Next page"
                        >
                          <ChevronRight className="w-4 h-4 text-gray-600" />
                        </button>
 
                        <button
                          onClick={goToLastPage}
                          disabled={currentPage === totalPages}
                          className="p-2 rounded-lg border border-gray-300 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
                          title="Last page"
                        >
                          <ChevronsRight className="w-4 h-4 text-gray-600" />
                        </button>
                      </div>
                    </div>
                  </div>
                )}
              </>

            ) : (
              <EmptyState searchQuery={searchQuery} />
            )}
          </>
        )}
      </div>

      {selectedTransaction && (
        <TransactionModal 
          transaction={selectedTransaction}
          onClose={handleCloseModal}
          categoryName={categoryMap[selectedTransaction.categoryID]}
          currencyCode={currencyMap[selectedTransaction.currencyID]}
          walletName={walletMap[selectedTransaction.walletID]}
        />
      )}

      {showDeleteModal && (
        <DeleteConfirmModal
          title="Delete Transactions?"
          message={`Are you sure you want to delete ${selectedIds.size} transaction${selectedIds.size > 1 ? 's' : ''}? This action cannot be undone.`}
          itemCount={selectedIds.size}
          onConfirm={handleConfirmDelete}
          onCancel={() => setShowDeleteModal(false)}
          loading={deleting}
        />
      )}

      </>
  );
}

function EmptyState({ searchQuery }) {
  return (
    <div className="text-center py-12">
      <div className="w-16 h-16 bg-gray-100 rounded-full flex items-center justify-center mx-auto mb-4">
        <Search className="w-8 h-8 text-gray-400" />
      </div>
      <p className="text-gray-600 font-medium">No transactions found</p>
      <p className="text-sm text-gray-500 mt-1">
        {searchQuery 
          ? "Try a different search term" 
          : "Start by adding your first transaction"}
      </p>
    </div>
  );
}