using UnityEngine;
using UnityEngine.UI;

public class StationManager : MonoBehaviour
{
    public GameObject[] stationPanels;
    public GameObject stationOverviewExtraPanel;
    public GameObject[] miniGames;
    public Button[] levelButtons;

    public AudioClip[] miniGameStartSounds;
    public AudioSource audioSource;
    public AudioClip errorAudioClip;
    public AudioClip successAudioClip;

    public GameObject[] miniGame1AdditionalObjects;
    public GameObject[] miniGame2AdditionalObjects;
    public GameObject[] miniGame3AdditionalObjects;
    public GameObject[] miniGame4AdditionalObjects;

    public MiniGameSocketChecker[] socketCheckers;

    public RepairStationCompletion repairStationCompletion;

    private int activeMiniGameIndex = -1;

    private void Start() { }

    public void OnStationButtonClicked(int index)
    {
        if (levelButtons[index].interactable)
        {
            if (RepairStationIntroduction.instance != null)
            {
                RepairStationIntroduction.instance.InterruptIntroAndHidePanels();
            }

            PlayMiniGameSound(index);
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

            switch (index)
            {
                case 0: ShowAdditionalObjects(miniGame1AdditionalObjects); break;
                case 1: ShowAdditionalObjects(miniGame2AdditionalObjects); break;
                case 2: ShowAdditionalObjects(miniGame3AdditionalObjects); break;
                case 3: ShowAdditionalObjects(miniGame4AdditionalObjects); break;
            }
        }
    }

    private void ShowAdditionalObjects(GameObject[] additionalObjects)
    {
        if (additionalObjects != null)
        {
            foreach (var obj in additionalObjects)
                if (obj != null) obj.SetActive(true);
        }
    }

    private void HideAdditionalObjects(GameObject[] additionalObjects)
    {
        if (additionalObjects != null)
        {
            foreach (var obj in additionalObjects)
                if (obj != null) obj.SetActive(false);
        }
    }

    public void OnMiniGameCompleted()
    {
        if (activeMiniGameIndex != -1)
        {
            var checker = socketCheckers[activeMiniGameIndex];
            if (checker != null && !checker.AreAllSocketsCorrect())
            {
                PlayErrorSound();
                return;
            }

            PanelStateController.instance.OnLevelCompleted(activeMiniGameIndex);
            miniGames[activeMiniGameIndex].SetActive(false);
            PlaySuccessSound();

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
    }

    private System.Collections.IEnumerator TriggerCompletionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        repairStationCompletion.TriggerCompletionManually();
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

    private void PlayErrorSound()
    {
        if (audioSource != null && errorAudioClip != null)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = errorAudioClip;
            audioSource.Play();
        }
    }

    private void PlaySuccessSound()
    {
        if (audioSource != null && successAudioClip != null)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = successAudioClip;
            audioSource.Play();
        }
    }
}
