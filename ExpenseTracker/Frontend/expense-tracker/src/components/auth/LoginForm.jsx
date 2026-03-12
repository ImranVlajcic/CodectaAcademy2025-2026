import { Link, useNavigate } from 'react-router-dom';
import { Mail, Lock, ArrowRight } from 'lucide-react';
import { authService } from '../../services/authService';
import toast from 'react-hot-toast';
import useForm from '../../hooks/useForm';
import Button from '../common/Button';
import Input from '../common/Input';
import ErrorAlert from '../common/ErrorAlert.jsx';

export default function LoginForm() {
  const navigate = useNavigate();

  const handleLogin = async (values) => {
    try {
      await authService.login(values.email, values.password);
      toast.success('Welcome back!');
      navigate('/dashboard');
    } catch (err) {
      console.error('Login error:', err);
      const errorMessage = err.response?.data?.detail || 
                          'Invalid email or password. Please try again.';
      throw new Error(errorMessage);
    }
  };

  const { values, errors, loading, handleChange, handleSubmit, setErrors } = useForm(
    { email: '', password: '' },
    handleLogin
  );

  return (
    <form onSubmit={handleSubmit} className="space-y-6">
      <ErrorAlert 
        message={errors.submit} 
        onClose={() => setErrors({ ...errors, submit: '' })}
      />

      <Input
        type="email"
        id="email"
        name="email"
        label="Email Address"
        placeholder="you@example.com"
        value={values.email}
        onChange={handleChange}
        icon={Mail}
        //required
      />

      <Input
        type="password"
        id="password"
        name="password"
        label="Password"
        placeholder="••••••••"
        value={values.password}
        onChange={handleChange}
        icon={Lock}
        //required
      />

      <Button
        type="submit"
        variant="primary"
        loading={loading}
        icon={ArrowRight}
      >
        Sign In
      </Button>

      <div className="text-center">
        <p className="text-gray-600">
          Don't have an account?{' '}
          <Link
            to="/register"
            className="text-blue-600 hover:text-blue-700 font-semibold hover:underline"
          >
            Create one
          </Link>
        </p>
      </div>
    </form>
  );
}