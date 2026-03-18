import { useNavigate, useLocation } from 'react-router-dom';
import toast from 'react-hot-toast';
import useTransactionForm from '../../hooks/useTransactionForm';
import Button from '../common/Button';
import Input from '../common/Input';
import ErrorAlert from '../common/ErrorAlert';
import CategorySelector from '../common/CategorySelector';
import CurrencySelector from '../common/CurrencySelector';
import transactionService from '../../services/transactionService';

export default function TransactionForm() {
  const navigate = useNavigate();
  const { state } = useLocation();
  const walletId = state?.walletId ?? '';

  const handleTransactionCreate = async (values) => {
    try {
      const transactionData = {
        walletID: values.walletId,
        categoryID: values.categoryId,
        currencyID: values.currencyId,
        amount: parseFloat(values.amount) || 0,
        transactionType: values.transactionType,
        description: values.description,
        transactionDate: values.transactionDate,
        transactionTime: values.transactionTime
      };

      await transactionService.create(transactionData);
      toast.success('Transaction created successfully!');
      navigate('/wallets');
    } catch (err) {
      console.error('Creation error:', err);
      const errorMessage = err.response?.data?.detail || 
                          err.response?.data?.errors?.[Object.keys(err.response.data.errors)[0]]?.[0] ||
                          'Failed to create transaction. Please try again.';
      throw new Error(errorMessage);
    }
  };

  const { values, errors, handleChange, handleSubmit, setErrors } = useTransactionForm(
    { 
      walletId,
      categoryId: '',
      currencyId: '',
      amount: '',
      transactionType: 'Cash',
      description: '',
      transactionDate: new Date().toISOString().split('T')[0], 
      transactionTime: new Date().toTimeString().slice(0, 5) 
    },
    handleTransactionCreate
  );

  return (
    <form onSubmit={handleSubmit} className="space-y-5 w-full max-w-sm mx-auto">
      <ErrorAlert 
        message={errors.submit} 
        onClose={() => setErrors({ ...errors, submit: '' })}
      />

      <div className="space-y-2">
        <label className="block text-sm font-semibold text-gray-700">
          Category <span className="text-red-500">*</span>
        </label>
        <CategorySelector
          value={values.categoryId}
          onChange={(categoryId) =>
            handleChange({
              target: { name: 'categoryId', value: categoryId },
            })
          }
        />
        {errors.categoryId && <p className="text-sm text-red-600">{errors.categoryId}</p>}
      </div>

      <div className="space-y-2">
        <label className="block text-sm font-semibold text-gray-700">
          Currency <span className="text-red-500">*</span>
        </label>
        <CurrencySelector
          value={values.currencyId}
          onChange={(currencyId) =>
            handleChange({
              target: { name: 'currencyId', value: currencyId },
            })
          }
        />
        {errors.currencyId && <p className="text-sm text-red-600">{errors.currencyId}</p>}
      </div>

      <div className="space-y-2">
        <label className="block text-sm font-semibold text-gray-700">
          Payment Method <span className="text-red-500">*</span>
        </label>
        <select
          name="transactionType"
          value={values.transactionType}
          onChange={handleChange}
          className="w-full px-4 py-3 bg-gray-50 border border-gray-200 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all"
        >
          <option value="Cash">Cash</option>
          <option value="Card">Card</option>
        </select>
      </div>

      <Input
        type="number"
        id="amount"
        name="amount"
        label="Amount"
        placeholder="100.00"
        step="0.01"
        value={values.amount}
        onChange={handleChange}
        error={errors.amount}
        required
      />

      <Input
        type="text"
        id="description"
        name="description"
        label="Description"
        placeholder="Grocery shopping"
        value={values.description}
        onChange={handleChange}
        error={errors.description}
      />

      <Input
        type="date"
        id="transactionDate"
        name="transactionDate"
        label="Date"
        value={values.transactionDate}
        onChange={handleChange}
        required
      />

      <Input
        type="time"
        id="transactionTime"
        name="transactionTime"
        label="Time"
        value={values.transactionTime}
        onChange={handleChange}
        required
      />

      <Button type="submit" variant="primary">
        Add Transaction
      </Button>
    </form>
  );
}