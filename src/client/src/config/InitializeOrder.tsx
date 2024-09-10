import React, { useEffect, useState } from 'react';
import axios from '../config/axiosConfig';

export default function InitializeOrder() {

  useEffect(() => {
    const initializeOrder = async () => {
      localStorage.removeItem('selectedProductIds');
      localStorage.removeItem('orderId');

      try {
        const response = await axios.post('/orders/create-new-order');
        const { id: orderId } = response.data;

        localStorage.setItem('orderId', orderId.toString());
      } catch (error) {
        console.error(error);
      }
    };

    initializeOrder();
  }, []);

  return null;
}
