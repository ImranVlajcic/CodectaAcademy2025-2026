import { Search, Plus } from 'lucide-react';
import toast from 'react-hot-toast';

export default function SearchBar({ searchQuery, setSearchQuery, onSearch }) {
  return (
    <div className="card mb-8">
      <div className="flex flex-col sm:flex-row gap-4">
        <form onSubmit={onSearch} className="flex-1 flex gap-2">
          <div className="relative flex-1">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 w-5 h-5 text-gray-400" />
            <input
              type="text"
              placeholder="Search by description or type..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              className="input-search"
            />
          </div>
          <button
            type="submit"
            className="btn-primary w-40"
          >
            Search
          </button>
        </form>
      </div>
    </div>
  );
}