using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;
using System.Collections.Generic;

public class P3Manager : MonoBehaviour
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

    [Header("Timing Settings")]
    public float startAudioDelaySeconds = 2.0f;

    [Header("Grabbable Objekte (werden nach Intro aktiv)")]
    public List<GameObject> objectsToManage;

    [Header("Debug")]
    public bool activateManually = false;

    private bool puzzleCompleted = false;
    private bool hasStarted = false;
    private bool hasTriggeredAudioPlayed = false;
    private bool startAudioFinished = false;

    void Start()
    {
        SetObjectsInitialState(false);
    }

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
        SetObjectsInitialState(true);
    }

    private void SetObjectsInitialState(bool enable)
    {
        foreach (var obj in objectsToManage)
        {
            if (obj == null) continue;

            var rb = obj.GetComponent<Rigidbody>();
            var grab = obj.GetComponent<XRGrabInteractable>();

            if (rb != null)
                rb.isKinematic = !enable;

            if (grab != null)
                grab.enabled = enable;
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
        return vrCameraTransform != null && triggerZone != null && triggerZone.bounds.Contains(vrCameraTransform.position);
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
