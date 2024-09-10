import React, { useState, useEffect } from 'react';
import axios from '../config/axiosConfig';
import Coin from '../components/Coin';
import '../assets/styles/Payment.css';
import { Link } from 'react-router-dom';

interface Coin {
  denomination: number;
  quantity: number; 
  amount: number;   
}

interface PaymentResponse {
  changeAmount: number;
  changeCoins: Record<string, number>;
  message: string;
  isChangeGiven: boolean;
}

export default function PaymentPage() {
  const [coins, setCoins] = useState<Coin[]>([]); 
  const [userCoins, setUserCoins] = useState<Record<number, number>>({});
  const [totalCost, setTotalCost] = useState<number>(0);
  const [totalInserted, setTotalInserted] = useState<number>(0);
  const [isPaymentSuccessful, setIsPaymentSuccessful] = useState<boolean>(false);
  const [change, setChange] = useState<number | null>(null);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [changeCoins, setChangeCoins] = useState<Record<number, number>>({
    1: 0,
    2: 0,
    5: 0,
    10: 0,
  });

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [coinsResponse, priceRangeResponse] = await Promise.all([
          axios.get('/payment/coins'),
          axios.get('/orders/sum', { params: { orderId: localStorage.getItem('orderId') } })
        ]);

        const fetchedCoins = coinsResponse.data;
        setCoins(fetchedCoins);
        setTotalCost(priceRangeResponse.data.totalSum);

        const initialUserCoins = fetchedCoins.reduce((acc: Record<number, number>, coin: Coin) => {
          acc[coin.denomination] = 0;
          return acc;
        }, {});
        setUserCoins(initialUserCoins);
      } catch (error) {
        console.error(error);
      }
    };

    fetchData();
  }, []);

  useEffect(() => {
    const total = Object.entries(userCoins).reduce(
      (acc, [denomination, amount]) => acc + Number(denomination) * amount, 0);
    setTotalInserted(total);
  }, [userCoins]);

  const handleQuantityChange = (denomination: number, delta: number) => {
    setUserCoins((prevUserCoins) => ({
      ...prevUserCoins,
      [denomination]: Math.max(0, (prevUserCoins[denomination] || 0) + delta)
    }));
  };

  const handlePayment = async () => {
    const orderId = localStorage.getItem('orderId');
    if (!orderId) {
      alert('Ошибка: не найден OrderId');
      return;
    }

    try {
      const response = await axios.post<PaymentResponse>('/payment/pay', {
        orderId,
        coins: userCoins 
      });

      const { changeAmount, changeCoins, message, isChangeGiven } = response.data;

      if (!isChangeGiven) {
        setErrorMessage(message);
        setIsPaymentSuccessful(false);
        return;
      }

      setChange(changeAmount);
      setChangeCoins({
        1: changeCoins['One'] || 0,
        2: changeCoins['Two'] || 0,
        5: changeCoins['Five'] || 0,
        10: changeCoins['Ten'] || 0,
      });

      setIsPaymentSuccessful(true);
      localStorage.removeItem('selectedProductIds');
      localStorage.removeItem('orderId');

      await initializeOrder();
    } catch (error) {
      console.error(error);
    }
  };

  const initializeOrder = async () => {
    try {
      const response = await axios.post('/orders/create-new-order');
      const { id: orderId } = response.data;
      localStorage.setItem('orderId', orderId.toString());
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <>
      <h1>Оплата</h1>

      {isPaymentSuccessful ? (
        <div>
          <h2>Спасибо за вашу покупку, пожалуйста, возьмите вашу сдачу</h2>
          {change && <p style={{color: "green"}}>Ваша сдача: {change} руб.</p>}
          <div className="change-coins">
            <h3>Выданные монеты:</h3>
            {Object.entries(changeCoins).map(([denomination, quantity]) => (
              <p key={denomination}>{denomination} руб. - {quantity} шт.</p>
            ))}
          </div>
          <Link to='/'>
            <button className="back-btn">Перейти к напиткам</button>
          </Link>
        </div>
      ) : (
        <>
          <div className="coins-list">
            {coins.map((coin) => (
              <Coin
                key={coin.denomination}
                denomination={coin.denomination}
                onQuantityChange={handleQuantityChange} 
              />
            ))}
          </div>

          <div className="summary">
            <p>Итоговая сумма: {totalCost} руб.</p>
            <p style={{ color: totalInserted < totalCost ? "red" : "green" }}>
              Вы внесли: {totalInserted} руб.
            </p>
          </div>

          {errorMessage && (
            <div className="error-message">
              <p>{errorMessage}</p>
            </div>
          )}

          <div className="payment-actions">
            <Link to='/order'>
              <button className="back-btn">Вернуться</button>
            </Link>
              <button
                className="pay-btn"
                onClick={handlePayment}
                disabled={totalInserted < totalCost}
              >
                Оплатить
              </button>
          </div>
        </>
      )}
    </>
  );
}
