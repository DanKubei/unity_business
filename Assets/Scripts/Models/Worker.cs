using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker
{
    public string Name { get { return name; } }
    public double Salary { get { return salary; } }
    public double Experience { get { return experience; } }
    public float WorkSpeed { get { return workSpeed; } }
    public float Responsibility { get { return responsibility; } }
    public float Competence { get { return competence; } }
    public float Mood { get { return mood; } }
    public float MoodSanction { get { return moodSanction; } }
    public float MoodPraise { get { return moodPraise; } }

    private string name;
    private double salary, experience;
    private float workSpeed, responsibility, competence, mood, moodSanction, moodPraise;

    public Worker(string name, double salary, double experience, float workSpeed,
        float responsibility, float competence, float mood = 0.5f, float moodSanction = 0.1f, float moodPraise = 0.1f)
    {
        this.name = name;
        if (salary < 0)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        this.salary = salary;
        this.experience = experience;
        if (workSpeed < 0 || workSpeed > 1)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        this.workSpeed = workSpeed;
        if (responsibility < 0 || responsibility > 1)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        this.responsibility = responsibility;
        if (competence < 0 || competence > 1)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        this.competence = competence;
        if (mood < 0 || mood > 1)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        this.mood = mood;
        if (moodPraise < 0 || moodPraise > 1)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        this.moodPraise = moodPraise;
        if (moodSanction < 0 || moodSanction > 1)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        this.moodSanction = moodSanction;
    }

    public void SetSalary(double newSalary)
    {
        if (newSalary < 0)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        float moodChanges = -(float)(1f - newSalary / salary);
        if (salary < newSalary)
        {
            moodChanges *= moodPraise;
        }
        else if (salary > newSalary)
        {
            moodChanges *= moodSanction;
        }
        ChangeMood(moodChanges);
        salary = newSalary;
    }

    public void ChangeMood(float value)
    {
        mood += value;
        if (mood > 1)
        {
            mood = 1;
        }
        if (mood < 0)
        {
            mood = 0;
        }
    }

    public void AddExperience(double experience)
    {
        if (experience < 0)
        {
            throw new System.ArgumentOutOfRangeException();
        }
        this.experience += experience;
    }
}
