using UnityEngine;
using UnityEngine.UI;

public class StationManager : MonoBehaviour
{
    public GameObject[] stationPanels;
    public GameObject[] miniGames;
    public Button[] levelButtons;

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
        }
    }
}
