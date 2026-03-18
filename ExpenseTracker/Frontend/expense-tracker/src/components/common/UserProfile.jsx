import { Mail, User, Phone, Save } from 'lucide-react';
import { authService } from '../../services/authService';
import toast from 'react-hot-toast';
import accountService from '../../services/accountservice.js';
import { useEffect, useState } from "react";
import Button from '../common/Button';
import Input from '../common/Input';
import ErrorAlert from '../common/ErrorAlert.jsx';

export default function UserProfile() {
  const currentUser = authService.getCurrentUser();
  
  const [values, setValues] = useState({
    username: '',
    realName: '',
    realSurname: '',
    phoneNumber: ''
  });
  
  const [email, setEmail] = useState(''); 
  const [originalData, setOriginalData] = useState(null); 
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const loadUser = async () => {
      try {
        const userData = await accountService.getById(currentUser.userId);
        
        setOriginalData(userData);
        
        setValues({
          username: userData.username || "",
          realName: userData.realName || "",
          realSurname: userData.realSurname || "",
          phoneNumber: userData.phoneNumber || "",
        });
        
        setEmail(userData.email || "");
      } catch (err) {
        console.error("Failed to fetch account info:", err);
        toast.error("Failed to load account information");
      }
    };

    loadUser();
  }, [currentUser.userId]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setValues(prev => ({ ...prev, [name]: value }));
    
    if (errors[name]) {
      setErrors(prev => ({ ...prev, [name]: '' }));
    }
  };

  const validate = () => {
    const newErrors = {};

    if (!values.username) {
      newErrors.username = 'Username is required';
    }
    if (!values.realName) {
      newErrors.realName = 'First name is required';
    }
    if (!values.realSurname) {
      newErrors.realSurname = 'Last name is required';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validate()) {
      toast.error('Please fix the errors');
      return;
    }

    if (!originalData) {
      toast.error('User data not loaded');
      return;
    }

    try {
      setLoading(true);

      await accountService.update(currentUser.userId, {
        username: values.username,
        email: originalData.email, 
        password: originalData.passwordHash, 
        realName: values.realName,
        realSurname: values.realSurname,
        phoneNumber: values.phoneNumber
      });

      toast.success('Account updated successfully!');
      setErrors({});
    } catch (err) {
      console.error('Update error:', err);
      const errorMessage = err.response?.data?.detail || 
                          err.response?.data?.errors?.[Object.keys(err.response.data.errors)[0]]?.[0] ||
                          'Update failed. Please try again.';
      setErrors({ submit: errorMessage });
      toast.error(errorMessage);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="card">
      <h2 className="text-2xl font-bold text-gray-900 mb-6">Account Information</h2>
      
      <form onSubmit={handleSubmit} className="space-y-6">
        <ErrorAlert 
          message={errors.submit} 
          onClose={() => setErrors({ ...errors, submit: '' })}
        />

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Email (Cannot be changed)
          </label>
          <div className="relative">
            <Mail className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-400" />
            <input
              type="email"
              value={email}
              disabled
              className="input-field pl-10 bg-gray-100 cursor-not-allowed opacity-75"
            />
          </div>
        </div>

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
            icon={User}
          />

          <Input
            type="tel"
            id="phoneNumber"
            name="phoneNumber"
            label="Phone Number (optional)"
            placeholder="+123456789"
            value={values.phoneNumber}
            onChange={handleChange}
            icon={Phone}
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
            id="realSurname"
            name="realSurname"
            label="Last Name"
            placeholder="Surname"
            value={values.realSurname}
            onChange={handleChange}
            error={errors.realSurname}
          />
        </div>

        <Button
          type="submit"
          variant="primary"
          loading={loading}
          icon={Save}
        >
          Save Changes
        </Button>
      </form>
    </div>
  );
}