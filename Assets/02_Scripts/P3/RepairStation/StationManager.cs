using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StationManager : MonoBehaviour
{
    public GameObject[] stationPanels;
    public GameObject stationOverviewExtraPanel;
    public GameObject[] miniGames;
    public Button[] levelButtons;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] miniGameIntroClips;
    public AudioClip[] miniGameButtonClickClips;
    public AudioClip[] miniGameErrorClips;
    public AudioClip[] miniGameSuccessClip1;
    public AudioClip[] miniGameSuccessClip2; 
    public AudioClip miniGame4SuccessClip;

    public GameObject[] miniGame1AdditionalObjects;
    public GameObject[] miniGame2AdditionalObjects;
    public GameObject[] miniGame3AdditionalObjects;
    public GameObject[] miniGame4AdditionalObjects;

    public MiniGameSocketChecker[] socketCheckers;
    public RepairStationCompletion repairStationCompletion;

    private int activeMiniGameIndex = -1;

    public void OnStationButtonClicked(int index)
    {
        if (levelButtons[index].interactable)
        {
            if (RepairStationIntroduction.instance != null)
                RepairStationIntroduction.instance.InterruptIntroAndHidePanels();

            HideStationPanels();
            ShowMiniGame(index);
        }
    }

    private void ShowStationPanels()
    {
        foreach (var panel in stationPanels)
            panel.SetActive(true);

        foreach (var miniGame in miniGames)
            miniGame.SetActive(false);

        if (stationOverviewExtraPanel != null)
            stationOverviewExtraPanel.SetActive(true);
    }

    private void HideStationPanels()
    {
        foreach (var panel in stationPanels)
            panel.SetActive(false);

        if (stationOverviewExtraPanel != null)
            stationOverviewExtraPanel.SetActive(false);
    }

    private void ShowMiniGame(int index)
    {
        if (index >= 0 && index < miniGames.Length)
        {
            miniGames[index].SetActive(true);
            activeMiniGameIndex = index;
            PlayAudio(miniGameIntroClips[index]);

            switch (index)
            {
                case 0: ShowAdditionalObjects(miniGame1AdditionalObjects); break;
                case 1: ShowAdditionalObjects(miniGame2AdditionalObjects); break;
                case 2: ShowAdditionalObjects(miniGame3AdditionalObjects); break;
                case 3: ShowAdditionalObjects(miniGame4AdditionalObjects); break;
            }
        }
    }

    private void ShowAdditionalObjects(GameObject[] objects)
    {
        if (objects == null) return;
        foreach (var obj in objects) if (obj != null) obj.SetActive(true);
    }

    private void HideAdditionalObjects(GameObject[] objects)
    {
        if (objects == null) return;
        foreach (var obj in objects) if (obj != null) obj.SetActive(false);
    }

    public void OnMiniGameButtonPressed()
    {
        if (activeMiniGameIndex < 0) return;

        PlayAudio(miniGameButtonClickClips[activeMiniGameIndex]);

        var checker = socketCheckers[activeMiniGameIndex];
        if (checker != null && !checker.AreAllSocketsCorrect())
        {
            PlayAudio(miniGameErrorClips[activeMiniGameIndex]);
            return;
        }

        StartCoroutine(PlaySuccessAndFinishMiniGame());
    }

    private IEnumerator PlaySuccessAndFinishMiniGame()
    {
        if (activeMiniGameIndex <= 2)
        {
            PlayAudio(miniGameSuccessClip1[activeMiniGameIndex]);
            yield return new WaitWhile(() => audioSource.isPlaying);

            PlayAudio(miniGameSuccessClip2[activeMiniGameIndex]);
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
        else if (activeMiniGameIndex == 3)
        {
            PlayAudio(miniGame4SuccessClip);
            yield return new WaitWhile(() => audioSource.isPlaying);
        }

        PanelStateController.instance.OnLevelCompleted(activeMiniGameIndex);
        miniGames[activeMiniGameIndex].SetActive(false);

        switch (activeMiniGameIndex)
        {
            case 0: HideAdditionalObjects(miniGame1AdditionalObjects); break;
            case 1: HideAdditionalObjects(miniGame2AdditionalObjects); break;
            case 2: HideAdditionalObjects(miniGame3AdditionalObjects); break;
            case 3: HideAdditionalObjects(miniGame4AdditionalObjects); break;
        }

        ShowStationPanels();

        if (activeMiniGameIndex == 3 && repairStationCompletion != null)
        {
            StartCoroutine(TriggerCompletionAfterDelay(2f));
        }

        activeMiniGameIndex = -1;
    }

    private IEnumerator TriggerCompletionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        repairStationCompletion.TriggerCompletionManually();
    }

    private void PlayAudio(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;

        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.clip = clip;
        audioSource.Play();
    }
}
