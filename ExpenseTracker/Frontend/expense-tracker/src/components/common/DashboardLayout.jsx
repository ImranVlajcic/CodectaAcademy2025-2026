import { LogOut, UserIcon } from 'lucide-react';
import { Link } from 'react-router-dom';

export default function DashboardLayout({ user, onLogout, children }) {
  return (
    <div className="page-container">
      <header className="header">
        <div className="header-content">
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-9">
              <div>
                <h1 className="text-2xl font-bold text-gray-900">Expense Tracker</h1>
                <p className="text-sm text-gray-600">
                  Welcome, {user?.realName || user?.username}!
                </p>
              </div>
              <div className = " font-bold flex items-center gap-5">
                <Link
                    to="/dashboard"
                    className="text-black-600 hover:text-black-700 font-semibold-medium "
                >
                    Dashboard
                </Link>
                <Link
                    to="/wallets"
                    className="text-black-600 hover:text-black-700 font-semibold-medium "
                >
                    Wallets
                </Link>
                <Link
                    to="/transactions"
                    className="text-black-600 hover:text-black-700 font-semibold-medium "
                >
                    Expenses
                </Link>
                <Link
                    to="/statistics"
                    className="text-black-600 hover:text-black-700 font-semibold-medium "
                >
                    Statistics
                </Link>
              </div>
            </div>
            <div className="flex items-center gap-4">
            <div className="space-x-3 pl3">
            <Link
              to="/userprofile"
              className="block hover:opacity-80 transition-opacity"
            >
            <UserIcon 
               className="w-8 h-8 rounded-full ring-2"
            />
            </Link>
            </div>
            <button onClick={onLogout} className="btn-logout">
              <LogOut className="w-4 h-4" />
              <span className="font-medium">Logout</span>
            </button>
            </div>
          </div>
        </div>
      </header>

      <div className="content-wrapper">
        {children}
      </div>
    </div>
  );
}