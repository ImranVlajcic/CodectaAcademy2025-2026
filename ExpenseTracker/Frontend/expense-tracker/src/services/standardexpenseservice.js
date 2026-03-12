import api from './api';

export const expenseService = {
  getAllByUser: async () => {
    const response = await api.get('/StandardExpense/mine')
    return response.data;
  },

  getAll: async () => {
    const response = await api.get('/StandardExpense');
    return response.data;
  },

  getById: async (id) => {
    const response = await api.get(`/StandardExpense/${id}`);
    return response.data;
  },

  create: async (standardExpenseData) => {
    const response = await api.post('/StandardExpense', standardExpenseData);
    return response.data;
  },

  update: async (id, standardExpenseData) => {
    const response = await api.put(`/StandardExpense/${id}`, standardExpenseData);
    return response.data;
  },

  delete: async (id) => {
    const response = await api.delete(`/StandardExpense/${id}`);
    return response.data;
  },
};

export default expenseService;