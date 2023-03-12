using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Main : MonoBehaviour
{

    [SerializeField] private Transform graphTypeChoiceButton, graphPeriodChoiceButton;
    [SerializeField] private ChoiceMenu choiceMenu;
    [SerializeField] private string[] graphTypes, graphPeriods;

    private ButtonContainer graphTypeButton, graphPeriodButton;

    private void Start()
    {
        graphPeriodButton = new ButtonContainer(graphPeriodChoiceButton);
        graphTypeButton = new ButtonContainer(graphTypeChoiceButton);

        graphPeriodButton.Button.onClick.AddListener(delegate { InvokeChoiceMenu(graphPeriods, graphPeriodButton.name); });
        graphTypeButton.Button.onClick.AddListener(delegate { InvokeChoiceMenu(graphTypes, graphPeriodButton.name); });

        choiceMenu.OnMenuChoice += OnMenuChoice;
        Worker[] workers =
            {
            new Worker("David", 10, 0, 0.5f, 0.5f, 0.5f),
            new Worker("Alice", 5, 10, 0.25f, 0.75f, 0.2f),
            new Worker("George", 25, 100, 0.75f, 0.6f, 0.9f)
        };
        Shift[] shifts =
            {
            new Shift("", 7*60, 19*60),
            new Shift("", 7*60, 19*60),
            new Shift("", 7*60, 19*60),
            new Shift("", 7*60, 19*60),
            new Shift("", 7*60, 19*60),
            new Shift("", 7*60, 17*60),
            new Shift("", 7*60, 17*60)
        };
        foreach (Shift shift in shifts)
        {
            shift.AddWorker(workers[Random.Range(0, workers.Length - 1)]);
        }
        Place place = new Place("BottomClint", 100, 10, 0.75f);
        Product[] inProducts =
            {
            new Product("Beef", 5),
            new Product("Bread", 1)
        };
        Product[] outProducts = 
            {
            new Product("Basic Burger", 15)
        };
        Business business = new Business("Burger Cafe", place, inProducts, outProducts);
        business.AddWorkers(workers);
        for (int i = 0; i < shifts.Length; i++)
        {
            business.SetShift(i, shifts[i]);
        }
        print(business.GetSaveData());
    }

    private void OnMenuChoice(int choice, string self, string id)
    {
        if (self != name)
        {
            return;
        }
        if (id == graphPeriodButton.name)
        {
            
        }
        if (id == graphTypeButton.name)
        {

        }
    }

    private void InvokeChoiceMenu(string[] input, string id)
    {
        choiceMenu.InvokeMenu(input, name, id);
    }

}
