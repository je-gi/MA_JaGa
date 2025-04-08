using UnityEngine;

public class P3Manager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip startAudioClip;
    public AudioClip completionAudioClip;

    public DisableGrabAndMakeKinematicOnSocket disableGrabAndMakeKinematicOnSocket;

    private bool puzzleCompleted = false;
    private bool hasStarted = false;

    void Update()
    {
        if (hasStarted && disableGrabAndMakeKinematicOnSocket != null && disableGrabAndMakeKinematicOnSocket.IsPuzzleCompleted() && !puzzleCompleted)
        {
            PlayCompletionAudio();
            puzzleCompleted = true;
        }
    }

    public void StartPuzzleExternally()
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
