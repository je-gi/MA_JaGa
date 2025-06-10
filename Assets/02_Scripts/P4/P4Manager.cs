using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class P4Manager : MonoBehaviour
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

    [Header("Start Sound Delay")]
    public float startDelay = 2f;

    [Header("VisibilityCallout Reference")]
    public VisibilityCallout visibilityCallout;

    private bool puzzleCompleted = false;
    private bool hasStarted = false;
    private bool hasTriggeredAudioPlayed = false;
    private bool startAudioFinished = false;

    private bool structureCalloutShown = false;
    private bool projectFolderCalloutShown = false;

    void Update()
    {
        if (activateManually && !hasStarted)
        {
            activateManually = false;
            StartCoroutine(StartPuzzleWithDelay());
        }

        if (hasStarted && disableGrabAndMakeKinematicOnSocket != null && disableGrabAndMakeKinematicOnSocket.IsPuzzleCompleted() && !puzzleCompleted)
        {
            PlayCompletionAudio();
            puzzleCompleted = true;

            if (visibilityCallout != null)
            {
                visibilityCallout.SetCalloutVisibility("StructureCallout", false);
                visibilityCallout.SetCalloutVisibility("ProjectFolderCallout", false);
                visibilityCallout.SetCalloutVisibility("GameViewCallout", false);
                visibilityCallout.SetCalloutVisibility("HierarchyCallout", false);
                visibilityCallout.SetCalloutVisibility("InspectorCallout", false);
            }
        }

        if (!hasTriggeredAudioPlayed && hasStarted && startAudioFinished && triggerZone != null && IsCameraInTrigger())
        {
            PlayTriggerAreaAudio();
            hasTriggeredAudioPlayed = true;

            if (visibilityCallout != null)
            {
                visibilityCallout.SetCalloutVisibility("StructureCallout", true);
                structureCalloutShown = true;
            }
        }

        if (structureCalloutShown && startAudioFinished && hasTriggeredAudioPlayed && !projectFolderCalloutShown && !audioSource.isPlaying)
        {
            if (visibilityCallout != null)
            {
                visibilityCallout.SetCalloutVisibility("ProjectFolderCallout", true);
                projectFolderCalloutShown = true;

                visibilityCallout.SetCalloutVisibility("StructureCallout", false);
                structureCalloutShown = false;
            }
        }
    }

    public void StartPuzzleExternally()
    {
        if (!hasStarted)
            StartCoroutine(StartPuzzleWithDelay());
    }

    private IEnumerator StartPuzzleWithDelay()
    {
        yield return new WaitForSeconds(startDelay);
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
            startAudioFinished = false;
            StartCoroutine(WaitForAudioEnd(startAudioClip.length));
        }
        else
        {
            startAudioFinished = true;
        }
    }

    private IEnumerator WaitForAudioEnd(float length)
    {
        yield return new WaitForSeconds(length);
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

    public bool IsCompletionAudioPlaying()
    {
        return audioSource != null && audioSource.isPlaying && audioSource.clip == completionAudioClip;
    }
}
