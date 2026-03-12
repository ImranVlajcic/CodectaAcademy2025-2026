import api from './api';

export const walletService = {
  getAll: async () => {
    const response = await api.get('/Category');
    return response.data;
  },

  getById: async (id) => {
    const response = await api.get(`/Category/${id}`);
    return response.data;
  },

  create: async (categoryData) => {
    const response = await api.post('/Category', categoryData);
    return response.data;
  },

  update: async (id, categoryData) => {
    const response = await api.put(`/Category/${id}`, categoryData);
    return response.data;
  },

  delete: async (id) => {
    const response = await api.delete(`/Category/${id}`);
    return response.data;
  },
};

export default transactionService;