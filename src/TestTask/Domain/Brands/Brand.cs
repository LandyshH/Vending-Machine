﻿namespace Domain.Brands;

public class Brand
{
    public int Id { get; set; } 
    public string Name { get; private set; }

    public Brand()
    {
        
    }
    public Brand(string name)
    {
        Name = name;
    }
}
