using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private Transform graphTypeChoiceButton, graphPeriodChoiseButton;

    private ButtonContainer graphTypeButton, graphPeriodButton;

    private void Start()
    {
        graphPeriodButton = new ButtonContainer(graphPeriodChoiseButton);
        graphTypeButton = new ButtonContainer(graphTypeChoiceButton);
    }
}
