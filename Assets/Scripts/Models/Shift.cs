using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shift
{
    public string Name { get { return name; } }
    public double Start { get { return start; } }
    public double End { get { return end; } }
    public List<Worker> Workers { get { return workers; } }

    private string name;
    private double start, end;
    private List<Worker> workers = new List<Worker>();

    private const int maxTime = 1440;

    public Shift(string name, double start, double end)
    {
        this.name = name;
        if (start < 0 || end < 0 || start > maxTime || end > maxTime)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        this.start = start;
        this.end = end;
    }

    public string GetTimeRange()
    {
        int startHours = Mathf.CeilToInt((float)start / 60);
        int startMinutes = (int)(start - startHours * 60);
        int endHours = Mathf.CeilToInt((float)end / 60);
        int endMinutes = (int)(end - endHours * 60);
        return string.Format("{0}:{1} - {2}:{3}", startHours, startMinutes, endHours, endMinutes);
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
