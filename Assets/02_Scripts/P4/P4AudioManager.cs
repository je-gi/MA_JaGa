using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class P4AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Hand 1 Objekte")]
    public GameObject h1p1;
    public GameObject h1p2;
    public GameObject h1p3;
    public GameObject h1p4;

    [Header("Hand 1 Sockets")]
    public XRSocketInteractor h1s1;
    public XRSocketInteractor h1s2;
    public XRSocketInteractor h1s3;
    public XRSocketInteractor h1s4;

    [Header("Hand 1 Audio")]
    public AudioClip h1p1GrabAudio;
    public AudioClip h1s1FilledAudio;
    public AudioClip h1p2GrabAudio;
    public AudioClip h1s2FilledAudio;
    public AudioClip h1p3GrabAudio;
    public AudioClip materialChangeAudio;
    public AudioClip h1s3FilledAudio;
    public AudioClip h1p4GrabAudio;
    public AudioClip shrinkAudio;
    public AudioClip h1s4FilledAudio;

    private bool h1p1Grabbed = false;
    private bool h1s1Filled = false;
    private bool h1p2Grabbed = false;
    private bool h1s2Filled = false;
    private bool h1p3Grabbed = false;
    private bool h1s3Filled = false;
    private bool h1p4Grabbed = false;
    private bool h1s4Filled = false;

    [Header("VisibilityCallout Reference")]
    public VisibilityCallout visibilityCallout;

    private bool h1s4FilledTriggered = false;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleHand1();
    }

    private void HandleHand1()
    {
        if (!h1p1Grabbed && IsGrabbed(h1p1))
        {
            PlayAudio(h1p1GrabAudio);
            h1p1Grabbed = true;

            visibilityCallout?.SetCalloutVisibility("GameViewCallout", true);
            visibilityCallout?.SetCalloutVisibility("HierarchyCallout", true);
        }

        if (!h1p3Grabbed && IsGrabbed(h1p3))
        {
            PlayAudio(h1p3GrabAudio);
            h1p3Grabbed = true;

            visibilityCallout?.SetCalloutVisibility("InspectorCallout", true);
        }
    }

    private bool IsGrabbed(GameObject obj)
    {
        var grab = obj?.GetComponent<XRGrabInteractable>();
        return grab != null && grab.isSelected;
    }

    private void PlayAudio(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
            audioSource.PlayOneShot(clip);
        }
    }

    public void DeactivateAllCallouts()
    {
        if (visibilityCallout == null) return;

        visibilityCallout.SetCalloutVisibility("GameViewCallout", false);
        visibilityCallout.SetCalloutVisibility("HierarchyCallout", false);
        visibilityCallout.SetCalloutVisibility("InspectorCallout", false);
    }
}
