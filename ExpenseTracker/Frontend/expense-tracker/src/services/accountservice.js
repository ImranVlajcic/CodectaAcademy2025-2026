import api from './api';

export const accountService = {
  getAll: async () => {
    const response = await api.get('/Account');
    return response.data;
  },

  getById: async (id) => {
    const response = await api.get(`/Account/${id}`);
    return response.data;
  },

  create: async (accountData) => {
    const response = await api.post('/Account', accountData);
    return response.data;
  },

  update: async (id, accountData) => {
    const response = await api.put(`/Account/${id}`, accountData);
    return response.data;
  },

  delete: async (id) => {
    const response = await api.delete(`/Account/${id}`);
    return response.data;
  },
};

export default accountService;