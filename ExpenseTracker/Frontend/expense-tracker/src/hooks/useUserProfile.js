import { useState } from 'react';
import toast from 'react-hot-toast';

export default function useUserProfile(initialValues, onSubmit){
  const [values, setValues] = useState(initialValues);
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);

  const validatePassword = (password) => {
    const minLength = password.length >= 8;
    const hasUpper = /[A-Z]/.test(password);
    const hasLower = /[a-z]/.test(password);
    const hasNumber = /\d/.test(password);
    const hasSpecial = /[!@#$%^&*]/.test(password);

    return {
      minLength,
      hasUpper,
      hasLower,
      hasNumber,
      hasSpecial,
      isValid: minLength && hasUpper && hasLower && hasNumber && hasSpecial,
    };
  };

  const passwordChecks = validatePassword(values.password || '');

  const handleChange = (e) => {
    const { name, value } = e.target;
    setValues((prev) => ({ ...prev, [name]: value }));
    
    if (errors[name]) {
      setErrors((prev) => ({ ...prev, [name]: '' }));
    }
  };

  const validate = () => {
    const newErrors = {};

    if (!values.username) newErrors.username = 'Username is required';
    if (!values.email) newErrors.email = 'Email is required';
    if (!/\S+@\S+\.\S+/.test(values.email)) newErrors.email = 'Email is invalid';
    if (!values.password) newErrors.password = 'Password is required';
    if (!passwordChecks.isValid) newErrors.password = 'Password does not meet requirements';
    if (values.password !== values.confirmPassword) {
      newErrors.confirmPassword = 'Passwords do not match';
    }
    if (!values.realName) newErrors.realName = 'First name is required';
    if (!values.realSurname) newErrors.realSurname = 'Last name is required';

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validate()) {
      toast.error('Please fix the errors');
      return;
    }

    setLoading(true);

    try {
      await onSubmit(values);
    } catch (error) {
      setErrors({ submit: error.message });
    } finally {
      setLoading(false);
    }
  };

  const reset = () => {
    setValues(initialValues);
    setErrors({});
    setLoading(false);
  };

  return {
    values,
    setValues,
    errors,
    loading,
    passwordChecks,
    handleChange,
    handleSubmit,
    setErrors,
    reset,
  };

}