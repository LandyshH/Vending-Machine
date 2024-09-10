import React from 'react';
import '../assets/styles/ChangeCoin.css';


interface CoinProps {
  denomination: number;
  quantity: number;
}

export default function ChangeCoin({ denomination, quantity } : CoinProps) {
  return (
    <div className="coin-item">
      {denomination} руб.  {quantity} шт.
    </div>
  );
};

