import React, { useState, useEffect } from 'react';
import axios from '../config/axiosConfig';
import '../assets/styles/OrderItem.css';

interface OrderItemProps {
  productId: number;
  name: string;
  price: number;
  quantity: number;
  imageUrl: string;
  onQuantityChange: (productId: number, delta: number) => void;
  onRemove: (productId: number) => void;
}

interface ProductDto {
  id: number;
  name: string;
  price: number;
  brand: string;
  imageUrl: string;
  amount: number; 
}

export default function OrderItem({ productId, name, price, quantity, imageUrl, onQuantityChange, onRemove }: OrderItemProps) {
  const [inputValue, setInputValue] = useState<string>(quantity.toString());
  const [maxQuantity, setMaxQuantity] = useState<number>(100); 
  const [brand, setBrand] = useState<string>('')

  useEffect(() => {
    const loadProductData = async () => {
      try {
        const response = await axios.get<ProductDto>(`/products/${productId}`);
        const product = response.data;
        setMaxQuantity(product.amount);
        setBrand(product.brand);
      } catch (error) {
        console.error(error);
      }
    };

    loadProductData();
  }, [productId]);

  useEffect(() => {
    setInputValue(quantity.toString());
  }, [quantity]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    
    const numericValue = Math.max(1, Math.min(Number(value), maxQuantity));
    if (!isNaN(numericValue)) {
      setInputValue(numericValue.toString());
      onQuantityChange(productId, numericValue - quantity);
    }
  };

  const handleBlur = () => {
    const numericValue = Math.max(1, Math.min(Number(inputValue), maxQuantity));
    setInputValue(numericValue.toString());

    if (numericValue !== quantity) {
      onQuantityChange(productId, numericValue - quantity);
    }
  };

  return (
    <div className="order-item">
      <img src={imageUrl} alt={name} className="order-item-image" />
      <div className="order-item-details">
        <span className="item-name">{name} {brand}</span>
        <div className="quantity-control">
          <button onClick={() => onQuantityChange(productId, -1)} disabled={quantity <= 1}>-</button>
          <input
            type="text"
            value={inputValue}
            onChange={handleInputChange}
            onBlur={handleBlur}
          />
          <button onClick={() => onQuantityChange(productId, 1)} disabled={quantity >= maxQuantity}>+</button>
        </div>
        <span className="item-price">{price} руб.</span>
        <button className="remove-btn" onClick={() => onRemove(productId)}>Удалить</button>
      </div>
    </div>
  );
}
