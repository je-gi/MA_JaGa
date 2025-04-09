using UnityEngine;
using UnityEngine.UI;

public class StationManager : MonoBehaviour
{
    [Header("Panels & Minispiele")]
    public GameObject[] stationPanels;
    public GameObject[] miniGames;
    public Button[] levelButtons;

    [Header("Sounds")]
    public AudioClip[] miniGameStartSounds;
    public AudioSource audioSource;

    [Header("Zusätzliche Objekte pro Minispiel")]
    public GameObject[] miniGame1AdditionalObjects;
    public GameObject[] miniGame2AdditionalObjects; 
    public GameObject[] miniGame3AdditionalObjects; 

    private int activeMiniGameIndex = -1;

    private void Start()
    {
        ShowStationPanels();
    }

    public void OnStationButtonClicked(int index)
    {
        if (levelButtons[index].interactable)
        {
            PlayMiniGameSound(index); 

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

    private void PlayMiniGameSound(int index)
    {
        if (audioSource != null && miniGameStartSounds != null && index >= 0 && index < miniGameStartSounds.Length)
        {
            AudioClip clip = miniGameStartSounds[index];
            if (clip != null)
            {
                if (audioSource.isPlaying)
                    audioSource.Stop();

                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }
}
