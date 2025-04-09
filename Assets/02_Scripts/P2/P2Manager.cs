using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections.Generic;

public class P2Manager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip startAudioClip;
    public AudioClip completionAudioClip;
    public AudioClip triggerAreaAudioClip;

    [Header("Trigger Zone")]
    public Collider triggerZone;

    [Header("Camera (VR)")]
    public Transform vrCameraTransform;

    [Header("Socket Checker")]
    public DisableGrabAndMakeKinematicOnSocket disableGrabAndMakeKinematicOnSocket;

    [Header("Debug")]
    public bool activateManually = false;

    private bool puzzleCompleted = false;
    private bool hasStarted = false;
    private bool hasTriggeredAudioPlayed = false;

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

        if (!hasTriggeredAudioPlayed && triggerZone != null && IsCameraInTrigger())
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

    private bool IsCameraInTrigger()
    {
        if (vrCameraTransform != null && triggerZone != null)
        {
            return triggerZone.bounds.Contains(vrCameraTransform.position);
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

    public bool IsPuzzleCompleted => puzzleCompleted;
}
