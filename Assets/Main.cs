using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Main : MonoBehaviour
{
    public enum graphType
    {
        Income,
        Profit,
        Expenses
    }
    public enum graphTime
    {
        Week,
        Month,
        Quarter,
        Year
    }

    public static graphType GraphType;
    public static graphTime GraphTime;


}
