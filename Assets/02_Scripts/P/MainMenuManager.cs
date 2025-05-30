using UnityEngine;
using UnityEngine.XR;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject soundPanel;
    public GameObject controlsPanel;

    private bool menuOpen = false;
    private InputDevice leftHand;
    private bool lastButtonState = false;

    void Start()
    {
        CloseAllPanels();
        leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
    }

    void Update()
    {
        if (!leftHand.isValid)
        {
            leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            Debug.Log("Reacquired left hand device");
        }

        bool menuButtonPressed = false;

        if (leftHand.TryGetFeatureValue(CommonUsages.menuButton, out menuButtonPressed))
        {
            Debug.Log($"Menu button pressed: {menuButtonPressed}");

            if (menuButtonPressed && !lastButtonState)
            {
                if (!menuOpen)
                {
                    Debug.Log("Opening main menu");
                    OpenMainMenu();
                }
                else
                {
                    Debug.Log("Closing all panels");
                    CloseAllPanels();
                }
            }

            lastButtonState = menuButtonPressed;
        }
    }

    public void OpenMainMenu()
    {
        menuOpen = true;
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        soundPanel.SetActive(false);
        controlsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        soundPanel.SetActive(false);
        controlsPanel.SetActive(false);
    }

    public void OpenSound()
    {
        settingsPanel.SetActive(false);
        soundPanel.SetActive(true);
    }

    public void OpenControls()
    {
        settingsPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void BackFromSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void BackFromSoundOrControls()
    {
        soundPanel.SetActive(false);
        controlsPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseAllPanels()
    {
        menuOpen = false;
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        soundPanel.SetActive(false);
        controlsPanel.SetActive(false);
    }
}
