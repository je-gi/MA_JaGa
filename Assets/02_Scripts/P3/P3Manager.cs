using UnityEngine;

public class P3Manager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip startAudioClip;
    public AudioClip completionAudioClip;

    public DisableGrabAndMakeKinematicOnSocket disableGrabAndMakeKinematicOnSocket;

    private bool puzzleCompleted = false;

    void Start()
    {
        if (disableGrabAndMakeKinematicOnSocket == null)
        {
            return;
        }

        PlayStartAudio();
    }

    void Update()
    {
        if (disableGrabAndMakeKinematicOnSocket != null && disableGrabAndMakeKinematicOnSocket.IsPuzzleCompleted() && !puzzleCompleted)
        {
            PlayCompletionAudio();
            puzzleCompleted = true;
        }
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
}
