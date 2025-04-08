using UnityEngine;

public class P2Manager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip startAudioClip;
    public AudioClip completionAudioClip;

    public DisableGrabAndMakeKinematicOnSocket disableGrabAndMakeKinematicOnSocket;

    public bool activateManually = false;

    private bool puzzleCompleted = false;
    private bool hasStarted = false;

    void Update()
    {
        if (activateManually && !hasStarted)
        {
            StartPuzzle();
            activateManually = false;
        }

        if (hasStarted && disableGrabAndMakeKinematicOnSocket != null && disableGrabAndMakeKinematicOnSocket.IsPuzzleCompleted() && !puzzleCompleted)
        {
            PlayCompletionAudio();
            puzzleCompleted = true;
        }
    }

    public void StartPuzzleExternally()
    {
        StartPuzzle();
    }

    private void StartPuzzle()
    {
        if (hasStarted) return;

        hasStarted = true;
        PlayStartAudio();
    }

    private void PlayStartAudio()
    {
        if (startAudioClip != null && audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = startAudioClip;
            audioSource.Play();
        }
    }

    private void PlayCompletionAudio()
    {
        if (completionAudioClip != null && audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = completionAudioClip;
            audioSource.Play();
        }
    }

    public bool IsPuzzleCompleted => puzzleCompleted;
}
