import api from './api';

export const walletService = {
  getAllByUser: async () => {
    const response = await api.get(`/Wallet/mine`)
    return response.data;
  },

  getAll: async () => {
    const response = await api.get('/Wallet');
    return response.data;
  },

  getById: async (id) => {
    const response = await api.get(`/Wallet/${id}`);
    return response.data;
  },

  create: async (walletData) => {
    const response = await api.post('/Wallet', walletData);
    return response.data;
  },

  update: async (id, walletData) => {
    const response = await api.put(`/Wallet/${id}`, walletData);
    return response.data;
  },

  delete: async (id) => {
    const response = await api.delete(`/Wallet/${id}`);
    return response.data;
  },
};

export default walletService;