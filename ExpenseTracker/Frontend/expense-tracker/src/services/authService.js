import api from './api';
import axios from 'axios';

const API_BASE_URL = 'http://localhost:8080/api';

export const authService = {
  
  register: async (userData) => {
    const response = await axios.post(`${API_BASE_URL}/authentication/register`, userData);
    const { accessToken, refreshToken, userId, username, email, realName, realSurname } = response.data;
    
    
    localStorage.setItem('accessToken', accessToken);
    localStorage.setItem('refreshToken', refreshToken);
    localStorage.setItem('user', JSON.stringify({ userId, username, email, realName, realSurname }));
    
    return response.data;
  },

  
  login: async (email, password) => {
    const response = await axios.post(`${API_BASE_URL}/authentication/login`, {
      email,
      password,
    });
    
    const { accessToken, refreshToken, userId, username, realName, realSurname } = response.data;
    
    
    localStorage.setItem('accessToken', accessToken);
    localStorage.setItem('refreshToken', refreshToken);
    localStorage.setItem('user', JSON.stringify({ 
      userId, 
      username, 
      email, 
      realName, 
      realSurname 
    }));
    
    return response.data;
  },

  
  logout: async () => {
    try {
      await api.post('/authentication/logout');
    } catch (error) {
      console.error('Logout error:', error);
    } finally {
      
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('user');
    }
  },


  getCurrentUser: () => {
    const userStr = localStorage.getItem('user');
    return userStr ? JSON.parse(userStr) : null;
  },

  
  isAuthenticated: () => {
    return !!localStorage.getItem('accessToken');
  },

  
  getMe: async () => {
    const response = await api.get('/authentication/me');
    return response.data;
  },
};

export default authService;