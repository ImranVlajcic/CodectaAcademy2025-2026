import api from './api';

export const currencyService = {
  getAll: async () => {
    const response = await api.get('/Currency');
    return response.data;
  },

  getById: async (id) => {
    const response = await api.get(`/Currency/${id}`);
    return response.data;
  },

  create: async (currencyData) => {
    const response = await api.post('/Currency', currencyData);
    return response.data;
  },

  update: async (id, currencyData) => {
    const response = await api.put(`/Currency/${id}`, currencyData);
    return response.data;
  },

  delete: async (id) => {
    const response = await api.delete(`/Currency/${id}`);
    return response.data;
  },
};

export default currencyService;