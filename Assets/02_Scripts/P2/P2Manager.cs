using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

public class P2Manager : MonoBehaviour
{
    [Header("Trigger Zone")]
    public Collider triggerZone;
    public Transform PlayerCamera;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip startAudioClip;
    public AudioClip completionAudioClip;
    public AudioClip triggerAreaAudioClip;

    [Header("Sockets")]
    public DisableGrabAndMakeKinematicOnSocket disableGrabAndMakeKinematicOnSocket;

    [Header("Timing")]
    public float startAudioDelaySeconds = 2.0f;

    [Header("Debug")]
    public bool activateManually = false;

    private bool puzzleCompleted = false;
    private bool hasStarted = false;
    private bool hasTriggeredAudioPlayed = false;
    private bool startAudioFinished = false;

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

        if (hasStarted && startAudioFinished && !hasTriggeredAudioPlayed && triggerZone != null && IsCameraInTrigger())
        {
            PlayTriggerAreaAudio();
            hasTriggeredAudioPlayed = true;
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
        StartCoroutine(PlayStartAudioWithDelay());
    }

    private IEnumerator PlayStartAudioWithDelay()
    {
        yield return new WaitForSeconds(startAudioDelaySeconds);

        if (startAudioClip != null && audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = startAudioClip;
            audioSource.Play();

            yield return new WaitWhile(() => audioSource.isPlaying);
        }

        startAudioFinished = true;
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

    private bool IsCameraInTrigger()
    {
        if (PlayerCamera != null && triggerZone != null)
        {
            return triggerZone.bounds.Contains(PlayerCamera.position);
        }
        return false;
    }

    private void PlayTriggerAreaAudio()
    {
        if (triggerAreaAudioClip != null && audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = triggerAreaAudioClip;
            audioSource.Play();
        }
    }

    public bool IsCompletionAudioPlaying()
    {
        return audioSource != null && audioSource.isPlaying && audioSource.clip == completionAudioClip;
    }

    public bool IsPuzzleCompleted => puzzleCompleted;
}
