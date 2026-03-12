import { Link, useNavigate } from 'react-router-dom';
import { Mail, Lock, ArrowRight } from 'lucide-react';
import { authService } from '../../services/authService';
import toast from 'react-hot-toast';
import useRegisterForm from '../../hooks/useRegisterForm';
import Button from '../common/Button';
import Input from '../common/Input';
import ErrorAlert from '../common/ErrorAlert.jsx';
import PasswordRequirement from './PasswordRequirement.jsx';

export default function RegisterForm(){
    const navigate = useNavigate();

    const handleRegistration = async (values) => {
    try {
      const { confirmPassword, ...registerData } = values;
      await authService.register(registerData);
      toast.success('Account created successfully!');
      navigate('/dashboard');
    } catch (err) {
      console.error('Registration error:', err);
      const errorMessage = err.response?.data?.detail || 
                          err.response?.data?.errors?.[Object.keys(err.response.data.errors)[0]]?.[0] ||
                          'Registration failed. Please try again.';
      setErrors({ submit: errorMessage });
      throw new Error(errorMessage);
    }
  };

  const { values, errors, loading, passwordChecks, handleChange, handleSubmit, setErrors } = useRegisterForm(
      { username: '', email: '', password: '', confirmPassword: '', realName: '', realSurname: '', phoneNumber: '' },
      handleRegistration
    );

    return(
    <form onSubmit={handleSubmit} className='space-y-6'>
        <ErrorAlert 
            message={errors.submit} 
            onClose={() => setErrors({ ...errors, submit: '' })}
        />
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <Input
            type="text"
            id="username"
            name="username"
            label="Username"
            placeholder="User1234"
            value={values.username}
            onChange={handleChange}
            error={errors.username}
        />

        <Input
            type="email"
            id="email"
            name="email"
            label="Email"
            placeholder="you@example.com"
            value={values.email}
            onChange={handleChange}
            error={errors.email}
            icon={Mail}
        />
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <Input
            type="text"
            id="realName"
            name="realName"
            label="First Name"
            placeholder="Name"
            value={values.realName}
            onChange={handleChange}
            error={errors.realName}
        />

        <Input
            type="text"
            id="realSurame"
            name="realSurname"
            label="Last Name"
            placeholder="Surname"
            value={values.realSurname}
            onChange={handleChange}
            error={errors.realSurname}
        />
        </div>
        <Input
            type="tel"
            id="phoneNumber"
            name="phoneNumber"
            label="Phone Number (optional)"
            placeholder="+123456789"
            value={values.phoneNumber}
            onChange={handleChange}
        />

        <div>
        <Input
            type="password"
            id="password"
            name="password"
            label="Password"
            placeholder="••••••••"
            value={values.password}
            onChange={handleChange}
            error={errors.password}
            icon = {Lock}
        />
        {values.password && (
          <div className="mt-3 space-y-2">
            <PasswordRequirement 
              met={passwordChecks.minLength} 
              text="At least 8 characters" 
            />
            <PasswordRequirement 
              met={passwordChecks.hasUpper} 
              text="One uppercase letter" 
            />
            <PasswordRequirement 
              met={passwordChecks.hasLower} 
              text="One lowercase letter" 
            />
            <PasswordRequirement 
              met={passwordChecks.hasNumber} 
              text="One number" 
            />
            <PasswordRequirement 
              met={passwordChecks.hasSpecial} 
              text="One special character (!@#$%^&*)" 
            />
          </div>
        )}
        </div>
        <Input
            type="password"
            id="confirmPassword"
            name="confirmPassword"
            label="Confirm Password"
            placeholder="••••••••"
            value={values.confirmPassword}
            onChange={handleChange}
            error={errors.confirmPassword}
            icon = {Lock}
        />

        <Button
            type="submit"
            variant="primary"
            loading={loading}
            icon={ArrowRight}
        >
            Create Account
        </Button>
        
        <div className="text-center">
        <p className="text-gray-600">
          Already have an account?{' '}
          <Link
            to="/login"
            className="text-blue-600 hover:text-blue-700 font-semibold hover:underline"
          >
            Log in
          </Link>
        </p>
      </div>
    </form>
    );
}

