// src/services/promocionesService.js
import axios from 'axios';

const API_URL = 'http://localhost:5194/api/PromocionesRandom';
const API_URLPRO = 'http://localhost:5194/api/Promociones';
const PRODUCTOS_API_URL = 'http://localhost:5194/api/Producto/ListadoProductos'; // Asegúrate de que este endpoint exista

const getPromocionesRandom = async () => {
  try {
    const response = await axios.get(API_URL);
    return response.data;
  } catch (error) {
    console.error('Error fetching promociones:', error);
    throw error; // Lanza el error para que pueda ser manejado en el componente
  }
};

const getProductos = async () => {
  try {
    const response = await axios.get(PRODUCTOS_API_URL);
    console.log(response.data)
    return response.data;
    
    return response.data;
  } catch (error) {
    console.error('Error fetching productos:', error);
    throw error; // Lanza el error para que pueda ser manejado en el componente
  }
};

const createPromocion = async (promocion) => {
  try {
    const response = await axios.post(`${API_URLPRO}/CreatePromocion`, promocion);
    return response.data;
  } catch (error) {
    console.error('Error creating promotion:', error);
    throw error;
  }
};
const sendPromotionEmail = async (promocionId, recipientType) => {
  try {
    const response = await axios.post(
      `${API_URLPRO}/EnviarCorreoPromocion?promocionId=${promocionId}&recipientType=${recipientType}`
    );
    return response.data;
  } catch (error) {
    console.error('Error sending promotion email:', error);
    throw error;
  }
};



export default {
  getPromocionesRandom,
  getProductos,
  createPromocion,
  sendPromotionEmail
};
