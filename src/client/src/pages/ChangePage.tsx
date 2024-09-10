import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import ChangeCoin from '../components/ChangeCoin'; 
import '../assets/styles/Change.css';

export default function ChangePage() {
  const [changeAmount, setChangeAmount] = useState<number | null>(null);
  const [changeCoins, setChangeCoins] = useState<Record<number, number>>({});

  useEffect(() => {
    const storedChangeAmount = localStorage.getItem('changeAmount');
    const storedChangeCoins = localStorage.getItem('changeCoins');

    if (storedChangeAmount && storedChangeCoins) {
      setChangeAmount(parseInt(storedChangeAmount, 10));
      setChangeCoins(JSON.parse(storedChangeCoins));
    }
  }, []);

  return (
    <div className="change-container">
      <h1>Спасибо за вашу покупку, пожалуйста, возьмите вашу сдачу</h1>
      <div className="change-amount" style={{ color: 'green' }}>
        {changeAmount !== null ? `${changeAmount} руб.` : 'Загрузка...'}
      </div>

      {Object.keys(changeCoins).length > 0 && (
        <div className="change-coins">
          <h2>Выданные монеты:</h2>
          <div>
            {Object.entries(changeCoins).map(([denomination, quantity]) => (
              <ChangeCoin key={denomination} denomination={Number(denomination)} quantity={quantity} />
            ))}
          </div>
        </div>
      )}

      <Link to="/">
        <button>Каталог напитков</button>
      </Link>
    </div>
  );
}
