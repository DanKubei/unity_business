using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place
{
    public string Name { get { return name; } }
    public double Rent { get { return rent; } }
    public double Passability { get { return passability; } }
    public float Risky { get { return risky; } }

    private string name;
    private double rent, passability;
    private float risky;

    public Place(string name, double rent, double passability, float risky)
    {
        this.name = name;
        if (rent < 0)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        this.rent = rent;
        if (passability < 0)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        this.passability = passability;
        if (risky < 0 || risky > 1)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        this.risky = risky;
    }

    public void SetRent(double newRent)
    {
        if (newRent < 0)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        rent = newRent;
    }

    public void SetPassability(double newPassability)
    {
        if (newPassability < 0)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        passability = newPassability;
    }

    public void SetRisky(float newRisky)
    {
        if (newRisky < 0 || newRisky > 1)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        risky = newRisky;
    }
}
