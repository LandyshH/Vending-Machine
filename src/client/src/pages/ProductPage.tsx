import React, { useState, useEffect } from 'react';
import axios from '../config/axiosConfig';
import '../assets/styles/Product.css';
import { Link } from 'react-router-dom';
import ProductCard from '../components/ProductCard';
import Filters from '../components/Filter';

interface Product {
    id: number;
    name: string;
    brand: string;
    price: number;
    imageUrl: string;
    available: boolean;
}

interface Brand {
    id: number;
    name: string;
}

export default function ProductPage() {
    const [products, setProducts] = useState<Product[]>([]);
    const [brands, setBrands] = useState<Brand[]>([]);
    const [selectedBrand, setSelectedBrand] = useState<number | string>('Все бренды');
    const [maxPrice, setMaxPrice] = useState<number>(0);
    const [minPrice, setMinPrice] = useState<number>(0);
    const [newMinPrice, setNewMinPrice] = useState<number>(0); 
    const [selectedProductIds, setSelectedProductIds] = useState<number[]>([]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const productResponse = await axios.get('/products');
                setProducts(productResponse.data);

                const brandResponse = await axios.get('/brands');
                setBrands([...brandResponse.data]);

                const storedSelectedProductIds = localStorage.getItem('selectedProductIds');
                if (storedSelectedProductIds) {
                    setSelectedProductIds(JSON.parse(storedSelectedProductIds));
                }

                if (selectedBrand === 'Все бренды') {
                    const priceRangeResponse = await axios.get('/products/price-range');
                    const { minPrice: serverMinPrice, maxPrice: serverMaxPrice } = priceRangeResponse.data;
                    setMinPrice(serverMinPrice);
                    setMaxPrice(serverMaxPrice);
                    setNewMinPrice(serverMinPrice);
                } else {
                    const priceRangeResponse = await axios.get('/products/price-range-brand', {
                        params: { brandId: selectedBrand }
                    });
                    const { minPrice: serverMinPrice, maxPrice: serverMaxPrice } = priceRangeResponse.data;
                    setMinPrice(serverMinPrice);
                    setMaxPrice(serverMaxPrice);
                    setNewMinPrice(serverMinPrice); 
                }
            } catch (error) {
                console.error(error);
            }
        };

        fetchData();
    }, [selectedBrand]);

    const handleBrandChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const brandValue = e.target.value;
        setSelectedBrand(brandValue === 'Все бренды' ? 'Все бренды' : parseInt(brandValue));
    };

    const handlePriceChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setNewMinPrice(parseInt(e.target.value, 10)); 
    };

    const handleProductSelect = async (id: number) => {
        const orderId = localStorage.getItem('orderId');
        if (!orderId) {
            console.error('orderId не найден в localStorage');
            return;
        }

        try {
            if (selectedProductIds.includes(id)) {
                await axios.post('/orders/delete', {
                    orderId: parseInt(orderId, 10),
                    productId: id,
                    amount: 1
                });

                setSelectedProductIds(prev => {
                    const updatedIds = prev.filter(productId => productId !== id);
                    localStorage.setItem('selectedProductIds', JSON.stringify(updatedIds));
                    return updatedIds;
                });
            } else {
                await axios.post('/orders/add-product', {
                    orderId: parseInt(orderId, 10),
                    productId: id,
                    amount: 1
                });

                setSelectedProductIds(prev => {
                    const updatedIds = [...prev, id];
                    localStorage.setItem('selectedProductIds', JSON.stringify(updatedIds));
                    return updatedIds;
                });
            }
        } catch (error) {
            console.error(error);
        }};

    const filteredProducts = products.filter(
        (product) =>
            (selectedBrand === 'Все бренды' || product.brand === brands
                                                                .find(b => b.id === selectedBrand)?.name) &&
            product.price >= newMinPrice && product.price <= maxPrice);

    return (
        <>
            <h1>Газированные напитки</h1>
            <div className="filter-celect-btn-container">
                <Filters
                    selectedBrand={selectedBrand.toString()}
                    minPrice={minPrice}
                    maxPrice={maxPrice}
                    onBrandChange={handleBrandChange}
                    onPriceChange={handlePriceChange}
                    brands={brands}
                    currentMinPrice={newMinPrice}
                />

                <div className="selected-product">
                    <Link to='/order'>
                        <button className={`button ${selectedProductIds.length > 0 ? 'selected' : 'disabled'}`}
                                disabled={selectedProductIds.length === 0}>
                                Выбрано {selectedProductIds.length > 0 ? selectedProductIds.length : ''}
                        </button>
                    </Link>
                </div>
            </div>

            <div className="product-grid">
                {filteredProducts.map((product) => (
                    <ProductCard
                        key={product.id}
                        product={product}
                        onSelect={handleProductSelect}
                        isSelected={selectedProductIds.includes(product.id)}
                    />))}
            </div>
        </>
    );
}
