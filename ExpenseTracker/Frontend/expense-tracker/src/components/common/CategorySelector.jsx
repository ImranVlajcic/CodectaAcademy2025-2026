import { useState, useEffect } from 'react';
import categoryService from '../../services/categoryService';

export default function CategorySelector({ value, onChange }) {
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const response = await categoryService.getAll();
        console.log('Categories Response:', response);
        
        const data = response.categories || (Array.isArray(response) ? response : []);
        setCategories(data || []);
      } catch (err) {
        console.error('Failed to fetch categories:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchCategories();
  }, []);

  return (
    <select
      className="w-full px-4 py-3 bg-gray-50 border border-gray-200 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all"
      value={value}
      onChange={(e) => onChange(Number(e.target.value))}
      disabled={loading}
    >
      <option value="">
        {loading ? 'Loading categories...' : 'Select category'}
      </option>

      {categories.map((category) => (
        <option key={category.categoryID} value={category.categoryID}>
          {category.categoryName}
        </option>
      ))}
    </select>
  );
}