using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ChoiceMenu : MonoBehaviour
{
    public delegate void MenuHandler(int output);
    public event MenuHandler OnMenuChoise;

    [SerializeField] private Transform buttonPrefab, menuTransform, contentTransform;

    private Image backgroundImage;
    private List<ButtonContainer> buttons = new List<ButtonContainer>();

    public void InvokeMenu(string[] options)
    {
        if (backgroundImage == null)
        {
            backgroundImage = GetComponent<Image>();
        }
        backgroundImage.enabled = true;
        menuTransform.gameObject.SetActive(true);
        UpdateButtons(options);
    }

    private void UpdateButtons(string[] options)
    {
        if (buttons.Count == 0)
        {
            CreateButtons(options.Length);
        }
        else
        {
            int difference = options.Length - buttons.Count;
            if (difference > 0)
            {
                CreateButtons(difference);
            }
            if (difference < 0)
            {
                DestroyButtons(difference);
            }
        }
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].Text.text = options[i];
        }
    }

    private void DestroyButtons(int count)
    {
        for (int i = 1; i <= count; i++)
        {
            int index = buttons.Count - i;
            Button button = buttons[index].Button;
            button.onClick.RemoveAllListeners();
            Destroy(button.gameObject);
            buttons.RemoveAt(index);
        }
    }

    private void CreateButtons(int count)
    {
        for (int i = 1; i <= count; i++)
        {
            ButtonContainer button = new ButtonContainer(Instantiate(buttonPrefab, contentTransform));
            buttons.Add(button);
            int index = buttons.Count - 1;
            button.Button.onClick.AddListener(delegate { OnButtonClick(index); });
        }
    }

    private void OnButtonClick(int index)
    {
        OnMenuChoise?.Invoke(index);
        backgroundImage.enabled = false;
        menuTransform.gameObject.SetActive(false);
    }
}
