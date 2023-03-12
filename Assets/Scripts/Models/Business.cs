using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Business
{
    public string Name { get { return name; } }
    public Place Place { get { return place; } }
    public List<Worker> Workers { get { return workers; } }
    public Shift[] Shifts { get { return shifts; } }
    public Product[] InProducts { get { return inProducts; } }
    public Product[] OutProducts { get { return outProducts; } }

    private string name;
    private Place place;
    private List<Worker> workers = new List<Worker>();
    private Shift[] shifts = new Shift[7];
    private Product[] inProducts, outProducts;

    public Business(string name, Place place, Product[] inProducts, Product[] outProducts)
    {
        this.name = name;
        if (place == null)
        {
            throw new System.ArgumentNullException();
        }
        this.place = place;
        this.inProducts = inProducts;
        if (outProducts.Length < 1)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        this.outProducts = outProducts;
    }

    public void SetShift(int index, Shift shift)
    {
        if (index < 0 || index > shifts.Length - 1)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        if (shift == null)
        {
            throw new System.ArgumentNullException();
        }
        foreach (Worker worker in shift.Workers)
        {
            bool hasWorker = false;
            foreach (Worker hiredWorker in workers)
            {
                if (worker == hiredWorker)
                {
                    hasWorker = true;
                    break;
                }
            }
            if (!hasWorker)
            {
                throw new System.ArgumentException("Wrong workers list");
            }
        }
        shifts[index] = shift;
    }

    public void AddWorker(Worker worker)
    {
        if (worker == null)
        {
            throw new System.ArgumentNullException();
        }
        workers.Add(worker);
    }

    public void AddWorkers(Worker[] workers)
    {
        if (workers.Length == 0)
        {
            throw new System.ArgumentNullException();
        }
        this.workers.AddRange(workers);
    }

    public void AddWorkers(List<Worker> workers)
    {
        if (workers.Count == 0)
        {
            throw new System.ArgumentNullException();
        }
        this.workers.AddRange(workers);
    }

    public void RemoveWorker(int index)
    {
        if (index < 0 || index >= workers.Count)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        workers.RemoveAt(index);
    }

    public void RemoveWorker(Worker worker)
    {
        workers.Remove(worker);
    }

    public void RemoveAllWorkers()
    {
        workers.Clear();
    }

    public string GetSaveData()
    {
        return JsonUtility.ToJson(this);
    }
}
