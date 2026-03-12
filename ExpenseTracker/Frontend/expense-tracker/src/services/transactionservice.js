import api from './api';

export const transactionService = {
  getAllByUser: async () => {
    const response = await api.get('/Transaction/mine')
    return response.data;
  },

  getAll: async () => {
    const response = await api.get('/Transaction');
    return response.data;
  },

  getById: async (id) => {
    const response = await api.get(`/Transaction/${id}`);
    return response.data;
  },

  create: async (transactionData) => {
    const response = await api.post('/Transaction', transactionData);
    return response.data;
  },

  update: async (id, transactionData) => {
    const response = await api.put(`/Transaction/${id}`, transactionData);
    return response.data;
  },

  delete: async (id) => {
    const response = await api.delete(`/Transaction/${id}`);
    return response.data;
  },
};

export default transactionService;