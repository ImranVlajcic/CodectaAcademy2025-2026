import { X, AlertTriangle, Trash2 } from 'lucide-react';

export default function DeleteConfirmModal({ 
  title = "Delete Items?",
  message = "Are you sure you want to delete the selected items? This action cannot be undone.",
  itemCount = 0,
  onConfirm, 
  onCancel, 
  loading = false
}) {
  return (
    <>
      <div 
        className="fixed inset-0 bg-gray bg-opacity-50 backdrop-blur-sm z-40 animate-in fade-in duration-200"
        onClick={onCancel}
      />

      <div className="fixed inset-0 z-50 flex items-center justify-center p-4 animate-in zoom-in duration-200">
        <div 
          className="bg-white rounded-2xl shadow-2xl max-w-md w-full"
          onClick={(e) => e.stopPropagation()}
        >
          <div className="p-6 border-b border-gray-200 bg-gradient-to-r from-red-50 to-orange-50">
            <div className="flex items-start justify-between">
              <div className="flex items-center gap-4">
                <div className="w-14 h-14 rounded-xl flex items-center justify-center bg-red-100">
                  <AlertTriangle className="w-8 h-8 text-red-600" />
                </div>
                <div>
                  <h2 className="text-2xl font-bold text-gray-900">{title}</h2>
                  <p className="text-sm font-medium text-red-700 mt-1">
                    This action cannot be undone
                  </p>
                </div>
              </div>
              <button
                onClick={onCancel}
                disabled={loading}
                className="p-2 hover:bg-white/50 rounded-lg transition-colors disabled:opacity-50"
              >
                <X className="w-6 h-6 text-gray-500" />
              </button>
            </div>
          </div>

          <div className="p-6">
            <div className="bg-red-50 border border-red-200 rounded-lg p-4 mb-4">
              <p className="text-gray-700 text-sm leading-relaxed">
                {message}
              </p>
            </div>

            {itemCount > 0 && (
              <div className="bg-gray-100 rounded-lg p-4">
                <p className="text-sm text-gray-600">
                  <span className="font-semibold text-gray-900">{itemCount}</span> item{itemCount > 1 ? 's' : ''} will be permanently deleted
                </p>
              </div>
            )}
          </div>

          <div className="p-6 border-t border-gray-200 bg-gray-50 flex gap-3">
            <button
              onClick={onCancel}
              disabled={loading}
              className="flex-1 py-3 px-4 bg-white border border-gray-300 text-gray-700 rounded-xl font-semibold hover:bg-gray-50 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
            >
              Cancel
            </button>
            <button
              onClick={onConfirm}
              disabled={loading}
              className="flex-1 py-3 px-4 bg-red-600 text-white rounded-xl font-semibold hover:bg-red-700 transition-colors disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center gap-2"
            >
              {loading ? (
                <>
                  <div className="w-5 h-5 border-2 border-white border-t-transparent rounded-full animate-spin" />
                  Deleting...
                </>
              ) : (
                <>
                  <Trash2 className="w-5 h-5" />
                  Delete {itemCount > 0 ? `(${itemCount})` : ''}
                </>
              )}
            </button>
          </div>
        </div>
      </div>
    </>
  );
}