import { useState, useEffect } from "react";
import { useNavigate } from 'react-router-dom';
import { authService } from '../services/authService';

export default function useAuth() {
  const [user, setUser] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    setUser(authService.getCurrentUser());
  }, []);

  const handleLogout = async () => {
    await authService.logout();
    toast.success('Logged out successfully');
    navigate('/login');
  };

  return { user, handleLogout };
}