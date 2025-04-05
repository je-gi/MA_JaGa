using UnityEngine;

public class P1Manager : MonoBehaviour
{
    [Header("Setup")]
    public GameObject initialPuzzleObject;

    [Header("Audio")]
    public AudioSource workshopOwnerAudioSource;
    public AudioClip clipIntro;
    public AudioClip clipOnSuccess;

    [Header("Socket Checker")]
    public SocketChecking socketChecker;

    [Header("Debug & Test")]
    public bool activateManually = false;

    private bool hasStarted = false;

    void Update()
    {
        if (activateManually && !hasStarted)
        {
            StartPuzzle();
            activateManually = false;
        }
    }

    public void StartPuzzle()
    {
        if (hasStarted) return;

        hasStarted = true;

        if (initialPuzzleObject != null)
            initialPuzzleObject.SetActive(true);

        if (clipIntro != null)
            PlayClip(clipIntro);

        if (socketChecker != null)
            socketChecker.OnSuccess += OnPuzzleSuccess;
    }

    private void OnPuzzleSuccess()
    {
        PlayClip(clipOnSuccess);
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip != null && workshopOwnerAudioSource != null)
        {
            workshopOwnerAudioSource.Stop();
            workshopOwnerAudioSource.clip = clip;
            workshopOwnerAudioSource.Play();
        }
    }

    private void OnDisable()
    {
        if (socketChecker != null)
            socketChecker.OnSuccess -= OnPuzzleSuccess;
    }
}
