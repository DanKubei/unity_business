using UnityEngine.UI;
using UnityEngine;

public class ButtonContainer : MonoBehaviour
{
    public Transform Transform { get; private set; }
    public Button Button { get; private set; }
    public Text Text { get; private set; }

    public ButtonContainer(Transform button)
    {
        Transform = button;
        Text = button.GetChild(0).GetComponent<Text>();
        Button = button.GetComponent<Button>();
    }
}
