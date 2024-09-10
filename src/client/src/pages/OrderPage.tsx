import React, { useState, useEffect } from 'react';
import axios from '../config/axiosConfig';
import OrderItem from '../components/OrderItem'; 
import { Link } from 'react-router-dom';
import '../assets/styles/Order.css';

interface OrderItemData {
  id: number;
  productDetails: {
    productId: number;
    productName: string;
    price: number;
    imageUrl: string;
  };
  amount: number;
}

export default function OrderPage() {
  const [cartItems, setOrderItems] = useState<OrderItemData[]>([]);
  const [totalAmount, setTotalAmount] = useState<number>(0);

  useEffect(() => {
    const fetchOrderItems = async () => {
      const orderId = localStorage.getItem('orderId');

      try {
        const response = await axios.get(`/orders/order-items`, {
          params: { orderId: Number(orderId) },
        });

        setOrderItems(response.data);
      } catch (error) {
        console.error(error);
      }};

    fetchOrderItems();
  }, []);

  useEffect(() => {
    const amount = cartItems.reduce((acc, item) => acc + item.productDetails.price * item.amount, 0);
    setTotalAmount(amount);
  }, [cartItems]);

  const handleQuantityChange = async (productId: number, delta: number) => {
    const updatedItems = cartItems.map((item) =>
      item.productDetails.productId === productId
        ? { ...item, amount: Math.max(1, item.amount + delta) }
        : item);

    const changedItem = updatedItems.find(item => item.productDetails.productId === productId);
    if (changedItem) {
      try {
        const orderId = localStorage.getItem('orderId');
        await axios.post('/orders/change-amount', {
          orderId: Number(orderId),
          productId,
          amount: changedItem.amount,
        });
      } catch (error) {
        console.error(error);
      }
    }
    
    setOrderItems(updatedItems);
  };

  const handleRemoveItem = async (productId: number) => {
    try {
      const orderId = localStorage.getItem('orderId');
      await axios.post('/orders/delete', {
        orderId: Number(orderId),
        productId,
        amount: 0, 
      });

      setOrderItems(cartItems.filter((item) => item.productDetails.productId !== productId));

      const storedSelectedProductIds = localStorage.getItem('selectedProductIds');
      if (storedSelectedProductIds) {
        const selectedProductIds = JSON.parse(storedSelectedProductIds);
        const updatedIds = selectedProductIds.filter((id: number) => id !== productId);
        localStorage.setItem('selectedProductIds', JSON.stringify(updatedIds));
      }
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="cart-container">
      <h1>Оформление заказа</h1>

      {cartItems.length === 0 ? (
        <>
                <p>У вас нет ни одного товара, вернитесь на страницу каталога.</p>
                <Link to='/'>
                  <button className="back-btn">Вернуться</button>
                </Link>
        </>
      ) : (
        <div className="order-items-list">
          {cartItems.map((item) => (
            <OrderItem
              key={item.productDetails.productId}
              productId={item.productDetails.productId}
              name={item.productDetails.productName}
              price={item.productDetails.price}
              quantity={item.amount}
              imageUrl={item.productDetails.imageUrl}
              onQuantityChange={handleQuantityChange}
              onRemove={handleRemoveItem}
            />
          ))}

          <div className="cart-summary">
            <Link to='/'>
              <button className="back-btn">Вернуться</button>
            </Link>
            <div className="pay-text-btn-container">
              <div className="total-amount">
                Итоговая сумма: {totalAmount} руб.
              </div>
              <Link to='/payment'>
                <button className="pay-btn">Оплата</button>
              </Link>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
