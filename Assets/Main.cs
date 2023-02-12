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
