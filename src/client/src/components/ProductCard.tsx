import React from 'react';
import '../assets/styles/Product.css';

interface Product {
    id: number;
    name: string;
    brand: string;
    price: number;
    imageUrl: string;
    available: boolean;
}

interface ProductCardProps {
    product: Product;
    onSelect: (id: number) => void;
    isSelected: boolean;
}

export default function ProductCard({ product, onSelect, isSelected }: ProductCardProps) {
    return (
        <div className="product-card">
            <img src={product.imageUrl} alt={product.name} className="product-image" />
            <div className="product-info">
                <h3 className="product-name">{product.name}</h3>
                <p className="product-price">{product.price} руб.</p>
                <button
                    className={
                        !product.available 
                            ? "button disabled" 
                            : isSelected 
                            ? "button selected" 
                            : "button available"
                    }
                    onClick={() => onSelect(product.id)}
                    disabled={!product.available}
                >
                    {!product.available ? "Закончился" : isSelected ? "Выбрано" : "Выбрать"}
                </button>
            </div>
        </div>
    );
}
