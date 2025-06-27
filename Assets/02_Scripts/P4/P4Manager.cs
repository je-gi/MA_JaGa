using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class P4Manager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip startAudioClip;
    public AudioClip completionAudioClip;
    public AudioClip triggerAreaAudioClip1;
    public AudioClip triggerAreaAudioClip2;

    public Collider triggerZone;
    public Transform vrCameraTransform;
    public DisableGrabAndMakeKinematicOnSocket disableGrabAndMakeKinematicOnSocket;

    public bool activateManually = false;
    public float startDelay = 2f;

    public VisibilityCallout visibilityCallout;
    public XRSocketInteractor h1Socket2;

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
            StartCoroutine(PlayTriggerAreaAudioSequence());
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
                visibilityCallout.SetCalloutVisibility("GameViewCallout", true);
                visibilityCallout.SetCalloutVisibility("HierarchyCallout", true);
                projectFolderCalloutShown = true;

                visibilityCallout.SetCalloutVisibility("StructureCallout", false);
                structureCalloutShown = false;
            }
        }

        if (projectFolderCalloutShown && h1Socket2 != null && h1Socket2.hasSelection)
        {
            visibilityCallout.SetCalloutVisibility("InspectorCallout", true);
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

    private IEnumerator PlayTriggerAreaAudioSequence()
    {
        if (audioSource == null) yield break;

        if (triggerAreaAudioClip1 != null)
        {
            audioSource.clip = triggerAreaAudioClip1;
            audioSource.Play();
            yield return new WaitForSeconds(triggerAreaAudioClip1.length);
        }

        if (triggerAreaAudioClip2 != null)
        {
            audioSource.clip = triggerAreaAudioClip2;
            audioSource.Play();
            yield return new WaitForSeconds(triggerAreaAudioClip2.length);
        }
    }

    public bool IsPuzzleCompleted => puzzleCompleted;

    public bool IsCompletionAudioPlaying()
    {
        return audioSource != null && audioSource.isPlaying && audioSource.clip == completionAudioClip;
    }
}
