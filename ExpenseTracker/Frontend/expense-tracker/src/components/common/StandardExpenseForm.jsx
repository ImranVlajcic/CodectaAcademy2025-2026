import { useNavigate, useLocation } from 'react-router-dom';
import toast from 'react-hot-toast';
import useStandardExpenseForm from '../../hooks/useStandardExpenseForm';
import Button from '../common/Button';
import Input from '../common/Input';
import ErrorAlert from '../common/ErrorAlert';
import standardExpenseService from '../../services/standardExpenseService';

export default function StandardExpenseForm() {
  const navigate = useNavigate();
  const { state } = useLocation();
  const walletId = state?.walletId ?? '';

  const calculateNextDate = (frequency) => {
    const today = new Date();
    const nextDate = new Date(today);

    switch (frequency) {
      case 'Daily':
        nextDate.setDate(today.getDate() + 1);
        break;
      case 'Weekly':
        nextDate.setDate(today.getDate() + 7);
        break;
      case 'Monthly':
        nextDate.setMonth(today.getMonth() + 1);
        break;
      case 'Yearly':
        nextDate.setFullYear(today.getFullYear() + 1);
        break;
      default:
        nextDate.setDate(today.getDate() + 1);
    }

    return nextDate.toISOString().split('T')[0]; 
  };

  const handleStandardExpenseCreate = async (values) => {
    try {
      const nextDate = calculateNextDate(values.frequency);

      const expenseData = {
        walletID: values.walletId,
        reason: values.reason,
        description: values.description,
        amount: parseFloat(values.amount) || 0,
        frequency: values.frequency,
        nextDate: nextDate
      };

      await standardExpenseService.create(expenseData);
      toast.success('Recurring expense created successfully!');
      navigate('/wallets/listing');
    } catch (err) {
      console.error('Creation error:', err);
      const errorMessage = err.response?.data?.detail || 
                          err.response?.data?.errors?.[Object.keys(err.response.data.errors)[0]]?.[0] ||
                          'Failed to create recurring expense. Please try again.';
      throw new Error(errorMessage);
    }
  };

  const { values, errors, handleChange, handleSubmit, setErrors } = useStandardExpenseForm(
    { 
      walletId,
      reason: '',
      description: '',
      amount: '',
      frequency: 'Monthly'
    },
    handleStandardExpenseCreate
  );

  return (
    <form onSubmit={handleSubmit} className="space-y-5 w-full max-w-sm mx-auto">
      <ErrorAlert 
        message={errors.submit} 
        onClose={() => setErrors({ ...errors, submit: '' })}
      />

      <Input
        type="text"
        id="reason"
        name="reason"
        label="Reason"
        placeholder="Rent, Subscription, etc."
        value={values.reason}
        onChange={handleChange}
        error={errors.reason}
        required
      />

      <Input
        type="text"
        id="description"
        name="description"
        label="Description"
        placeholder="Netflix monthly subscription"
        value={values.description}
        onChange={handleChange}
        error={errors.description}
      />

      <Input
        type="number"
        id="amount"
        name="amount"
        label="Amount"
        placeholder="15.00"
        step="0.01"
        value={values.amount}
        onChange={handleChange}
        error={errors.amount}
        required
      />

      <div className="space-y-2">
        <label className="block text-sm font-semibold text-gray-700">
          Frequency <span className="text-red-500">*</span>
        </label>
        <select
          name="frequency"
          value={values.frequency}
          onChange={handleChange}
          className="w-full px-4 py-3 bg-gray-50 border border-gray-200 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all"
        >
          <option value="Daily">Daily</option>
          <option value="Weekly">Weekly</option>
          <option value="Monthly">Monthly</option>
          <option value="Yearly">Yearly</option>
        </select>
      </div>

      {values.frequency && (
        <div className="p-3 bg-blue-50 border border-blue-200 rounded-xl">
          <p className="text-sm text-blue-800">
            Next occurrence: <strong>{calculateNextDate(values.frequency)}</strong>
          </p>
        </div>
      )}

      <Button type="submit" variant="primary">
        Add Recurring Expense
      </Button>
    </form>
  );
}