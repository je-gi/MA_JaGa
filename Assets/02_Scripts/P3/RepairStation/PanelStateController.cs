using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelStateController : MonoBehaviour
{
    public Button[] levelButtons;
    public Sprite lockedIcon;
    public Sprite unlockedIcon;
    public Sprite completedIcon;
    public TextMeshProUGUI[] buttonTexts;
    public Image[] buttonIcons;

    private enum PanelState { Locked, Unlocked, Completed }
    private PanelState[] panelStates;

    public static PanelStateController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        panelStates = new PanelState[levelButtons.Length];
        panelStates[0] = PanelState.Unlocked;

        for (int i = 1; i < panelStates.Length; i++)
        {
            panelStates[i] = PanelState.Locked;
        }

        UpdateButtonStates();
    }

    public void UpdateButtonStates()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            Button button = levelButtons[i];
            TextMeshProUGUI customText = buttonTexts[i];
            Image customIcon = buttonIcons[i];

            switch (panelStates[i])
            {
                case PanelState.Locked:
                    customIcon.sprite = lockedIcon;
                    customText.text = "Gesperrt";
                    button.interactable = false;
                    break;
                case PanelState.Unlocked:
                    customIcon.sprite = unlockedIcon;
                    customText.text = "Offen";
                    button.interactable = true;
                    break;
                case PanelState.Completed:
                    customIcon.sprite = completedIcon;
                    customText.text = "Abgeschlossen";
                    button.interactable = false;
                    break;
            }
        }
    }

    public void OnLevelCompleted(int index)
    {
        panelStates[index] = PanelState.Completed;
        if (index + 1 < panelStates.Length)
            panelStates[index + 1] = PanelState.Unlocked;

        UpdateButtonStates();
    }
}
