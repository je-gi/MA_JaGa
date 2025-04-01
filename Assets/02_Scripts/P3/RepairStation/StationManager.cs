using UnityEngine;
using UnityEngine.UI;

public class StationManager : MonoBehaviour
{
    public GameObject[] stationPanels;
    public GameObject[] miniGames;
    public Button[] levelButtons;

    // Arrays für GameObjects, die pro Minispiel ausgeblendet werden sollen
    public GameObject[] miniGame1AdditionalObjects; // Für Minispiel 1
    public GameObject[] miniGame2AdditionalObjects; // Für Minispiel 2
    public GameObject[] miniGame3AdditionalObjects; // Für Minispiel 3
    // Füge hier beliebig viele Minispiele und ihre zusätzlichen Objekte hinzu

    private int activeMiniGameIndex = -1;

    private void Start()
    {
        ShowStationPanels();
    }

    public void OnStationButtonClicked(int index)
    {
        if (levelButtons[index].interactable)
        {
            HideStationPanels();
            ShowMiniGame(index);
        }
    }

    private void ShowStationPanels()
    {
        foreach (GameObject panel in stationPanels)
        {
            panel.SetActive(true);
        }

        foreach (GameObject miniGame in miniGames)
        {
            miniGame.SetActive(false);
        }
    }

    private void HideStationPanels()
    {
        foreach (GameObject panel in stationPanels)
        {
            panel.SetActive(false);
        }
    }

    private void ShowMiniGame(int index)
    {
        if (index >= 0 && index < miniGames.Length)
        {
            miniGames[index].SetActive(true);
            activeMiniGameIndex = index;
        }
    }

    public void OnMiniGameCompleted()
    {
        if (activeMiniGameIndex != -1)
        {
            PanelStateController.instance.OnLevelCompleted(activeMiniGameIndex);
            miniGames[activeMiniGameIndex].SetActive(false);
            ShowStationPanels();

            // Hier können wir nun je nach Minispiel die spezifischen Objekte ausblenden
            switch (activeMiniGameIndex)
            {
                case 0:
                    HideAdditionalObjects(miniGame1AdditionalObjects);
                    break;
                case 1:
                    HideAdditionalObjects(miniGame2AdditionalObjects);
                    break;
                case 2:
                    HideAdditionalObjects(miniGame3AdditionalObjects);
                    break;
                    // Füge hier weitere Minispiele hinzu, falls nötig
            }
        }
    }

    private void HideAdditionalObjects(GameObject[] additionalObjects)
    {
        if (additionalObjects != null)
        {
            foreach (GameObject obj in additionalObjects)
            {
                if (obj != null)
                    obj.SetActive(false);
            }
        }
    }
}
