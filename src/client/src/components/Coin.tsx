import React, { useState, useEffect } from 'react';
import '../assets/styles/Coin.css';

interface CoinProps {
  denomination: number;
  onQuantityChange: (denomination: number, delta: number) => void;
}

export default function Coin({ denomination, onQuantityChange }: CoinProps) {
  const [inputValue, setInputValue] = useState<string>('0');
  const [previousValue, setPreviousValue] = useState<number>(0);


  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    const numericValue = Number(value);

    if (value === '' || (numericValue >= 0 && !isNaN(numericValue))) {
      setInputValue(value);
    }
  };

  const handleBlur = () => {
    const numericValue = Number(inputValue);
    const clampedValue = Math.max(0, Math.min(numericValue, 1000)); 
    const delta = clampedValue - previousValue; 
    onQuantityChange(denomination, delta); 
    setInputValue(clampedValue.toString());
    setPreviousValue(Number(inputValue));
  };

  const handleIncrement = () => {
    const numericValue = Number(inputValue);
    const newValue = numericValue + 1; 
    const delta = newValue - numericValue; 
    onQuantityChange(denomination, delta); 
    setInputValue(newValue.toString());
    setPreviousValue(newValue);
  };

  const handleDecrement = () => {
    const numericValue = Number(inputValue);
    const newValue = numericValue - 1; 
    const delta = newValue - numericValue; 
    onQuantityChange(denomination, delta); 
    setInputValue(newValue.toString());
    setPreviousValue(newValue);
  };

  return (
    <div className="coin-item">
      <span className="coin-denomination">{denomination} руб.</span>
      <div className="quantity-control">
        <button onClick={handleDecrement} disabled={Number(inputValue) <= 0}>-</button>
        <input className="coin-amount-input"
          type="text"
          value={inputValue}  
          onChange={handleInputChange}
          onBlur={handleBlur}
        />
        <button onClick={handleIncrement} disabled={Number(inputValue) >= 100}>+</button>
      </div>
      <span className="coin-total">{denomination * Number(inputValue)} руб.</span>
    </div>
  );
}
