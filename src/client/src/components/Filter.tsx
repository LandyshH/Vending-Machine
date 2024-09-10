import React from 'react';
import '../assets/styles/Filters.css';

interface FiltersProps {
    selectedBrand: string;
    minPrice: number;
    maxPrice: number;
    onBrandChange: (e: React.ChangeEvent<HTMLSelectElement>) => void;
    onPriceChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
    brands: { id: number, name: string }[];
    currentMinPrice: number;
}

export default function Filters({
    selectedBrand,
    minPrice,
    maxPrice,
    onBrandChange,
    onPriceChange,
    brands,
    currentMinPrice
}: FiltersProps) {
    return (
        <div className="filters">
            <label className="custom-select">
                Выберите бренд
                <select value={selectedBrand} onChange={onBrandChange}>
                    <option value="Все бренды">Все бренды</option>
                    {brands.map((brand) => (
                        <option key={brand.id} value={brand.id}>
                            {brand.name}
                        </option>
                    ))}
                </select>
            </label>

            <div className="slider-container">
                <label htmlFor="price-slider">Стоимость</label>
                <div className="slider-values">
                    <span>{currentMinPrice || 0} руб.</span>
                    <span>{maxPrice} руб.</span>
                </div>
            <input 
                type="range" 
                id="price-slider" 
                min={minPrice || 0}
                max={maxPrice || 0}
                value={currentMinPrice || minPrice} 
                className="slider" 
                onChange={onPriceChange}
            />
            </div>
        </div>
    );
}
