import './App.css';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import ProductPage from './pages/ProductPage';
import OrderPage from './pages/OrderPage';
import ChangePage from './pages/ChangePage';
import PaymentPage from './pages/PaymentPage';
import InitializeOrder from './config/InitializeOrder';

const router = createBrowserRouter([
  {
    path: '/',
    element: <ProductPage />,
    errorElement: <div>Not Found</div>
  },
  {
    path: '/order',
    element: <OrderPage />
  },
  {
    path: '/payment',
    element: <PaymentPage />
  },
  {
    path: '/change',
    element: <ChangePage />
  },
]);

function App() {
  return (
    <div className="App">
      <InitializeOrder />
      <RouterProvider router={router} />
    </div>
  );
}

export default App;
