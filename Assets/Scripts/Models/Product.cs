using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product
{
    public string Name { get { return name; } }
    public double Price { get { return price; } }

    private string name;
    private double price;

    public Product(string name, double price)
    {
        this.name = name;
        this.price = price;
    }

    public void SetPrice(double newPrice)
    {
        if (newPrice < 0)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        price = newPrice;
    }

    public string GetSaveData()
    {
        return JsonUtility.ToJson(this);
    }
}
