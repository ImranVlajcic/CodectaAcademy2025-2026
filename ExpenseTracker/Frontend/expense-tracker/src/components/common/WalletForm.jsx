import { useNavigate } from 'react-router-dom';
import toast from 'react-hot-toast';
import useWalletForm from '../../hooks/useWalletForm.js';
import Button from '../common/Button';
import Input from '../common/Input';
import ErrorAlert from '../common/ErrorAlert.jsx';
import CurrencySelector from './CurrencySelector.jsx';
import walletService from '../../services/walletservice.js';

export default function WalletForm() {
  const navigate = useNavigate();

  const handleWalletCreate = async (values) => {

    try {
      await walletService.create(values)
      toast.success('Created new Wallet');
      navigate('/wallets');
    } catch (err) {
      console.error('Creation error:', err);
      const errorMessage = err.response?.data?.detail || 
                          'Missing or invalid input. Please try again.';
      throw new Error(errorMessage);
    }
  };

  const { values, errors, handleChange, handleSubmit, setErrors } = useWalletForm(
    { purpose: '', balance: '', currencyId:""},
    handleWalletCreate
  );

  return (
    <form onSubmit={handleSubmit} className="space-y-5 w-60 align-center space-y-5 w-full max-w-sm mx-auto">
      <ErrorAlert 
        message={errors.submit} 
        onClose={() => setErrors({ ...errors, submit: '' })}
      />

      <Input
        type="text"
        id="purpose"
        name="purpose"
        label="Purpose (Name)"
        placeholder="Savings"
        value={values.purpose}
        onChange={handleChange}
      />

      <Input
        type="number"
        id="balance"
        name="balance"
        label="Initial balance"
        placeholder="130.00"
        value={values.balance}
        onChange={handleChange}
        //required
      />

      <div className='align-center'>
        <CurrencySelector
            value={values.currencyId}
            onChange={(currencyId) =>
                handleChange({
                target: { name: "currencyId", value: currencyId },
                })
            }
        />
      </div>

      <Button
        type="submit"
        variant="primary"
      >
        Create Wallet
      </Button>
    </form>
  );
}