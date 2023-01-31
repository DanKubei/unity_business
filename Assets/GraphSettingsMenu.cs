using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GraphSettingsMenu : MonoBehaviour
{
    public delegate void SettingsEvent(Main.graphTime time, Main.graphType type);
    public static event SettingsEvent GraphSettingsChange;

    public enum MenuType
    {
        TimeChoice,
        TypeChoice
    }

    [SerializeField] private Transform buttonPrefab, MenuTransform, ContentTransform;
    [SerializeField] private string[] timeTexts, typeTexts;

    private List<ButtonContainer> _buttons = new List<ButtonContainer>();
    private Image image;
    private MenuType currentType;

    public void OpenMenu(MenuType menuType)
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
        image.enabled = true;
        MenuTransform.gameObject.SetActive(true);
        currentType = menuType;
        int menuLength = (currentType == MenuType.TimeChoice ? timeTexts.Length : typeTexts.Length);
        CreateButtons(menuLength);
        switch(currentType)
        {
            case MenuType.TimeChoice:
                ChangeValues(timeTexts);
                break;
            case MenuType.TypeChoice:
                ChangeValues(typeTexts);
                break;
        };
    }

    private void OnButtonClick(int index)
    {
        switch (currentType) {
            case MenuType.TimeChoice:
                GraphSettingsChange?.Invoke((Main.graphTime)index, Main.GraphType);
                break;
            case MenuType.TypeChoice:
                GraphSettingsChange?.Invoke(Main.GraphTime, (Main.graphType)index);
                break;
        }
        image.enabled = false;
        MenuTransform.gameObject.SetActive(false);
    }

    private void ChangeValues(string[] texts)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            _buttons[i].Text.text = texts[i];
        }
    }

    private void CreateButtons(int menuLength)
    {
        if (_buttons.Count > 0)
        {
            int difference = _buttons.Count - menuLength;
            if (difference > 0)
            {
                for (int i = 1; i >= difference; i++)
                {
                    _buttons[_buttons.Count - i].Button.onClick.RemoveAllListeners();
                    Destroy(_buttons[_buttons.Count - i].gameObject);
                }
                _buttons.RemoveRange(_buttons.Count - difference, difference);
            }
            else if (difference < 0)
            {
                for (int i = 0; i < -difference; i++)
                {
                    CreateButton(i);
                }
            }
        }
        else
        {
            for (int i = 0; i < menuLength; i++)
            {
                CreateButton(i);
            }
        }
    }

    private void CreateButton(int index)
    {
        _buttons.Add(new ButtonContainer(Instantiate(buttonPrefab, ContentTransform)));
        int i = index;
        _buttons[index].Button.onClick.AddListener(delegate { OnButtonClick(i); });
    }
}
