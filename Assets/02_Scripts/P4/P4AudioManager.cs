using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class P4AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource;

    // ---------------------------- //
    //           HAND 1            //
    // ---------------------------- //
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

    // ---------------------------- //
    //           HAND 2            //
    // ---------------------------- //
    [Header("Hand 2 Start")]
    public AudioClip hand2StartAudio;
    private bool hand2StartPlayed = false;

    [Header("Hand 2 Sockets")]
    public XRSocketInteractor h2s1;
    public XRSocketInteractor h2s2;
    public XRSocketInteractor h2s3;
    public XRSocketInteractor h2s4;

    [Header("Hand 2 Audio")]
    public AudioClip h2s1FilledAudio;
    public AudioClip h2s2FilledAudio;
    public AudioClip h2s3FilledAudio;
    public AudioClip h2s4FilledAudio;

    private bool h2s1Filled = false;
    private bool h2s2Filled = false;
    private bool h2s3Filled = false;
    private bool h2s4Filled = false;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleHand1();
        HandleHand2();
    }

    private void HandleHand1()
    {
        if (!h1p1Grabbed && IsGrabbed(h1p1))
        {
            PlayAudio(h1p1GrabAudio);
            h1p1Grabbed = true;
        }

        if (h1p1Grabbed && !h1s1Filled && h1s1 != null && h1s1.hasSelection)
        {
            PlayAudioOnce(ref h1s1Filled, h1s1FilledAudio);
        }

        if (h1s1Filled && !h1p2Grabbed && IsGrabbed(h1p2))
        {
            PlayAudio(h1p2GrabAudio);
            h1p2Grabbed = true;
        }

        if (h1p2Grabbed && !h1s2Filled && h1s2 != null && h1s2.hasSelection)
        {
            PlayAudioOnce(ref h1s2Filled, h1s2FilledAudio);
        }

        if (h1s2Filled && !h1p3Grabbed && IsGrabbed(h1p3))
        {
            PlayAudio(h1p3GrabAudio);
            PlayAudio(materialChangeAudio);
            h1p3Grabbed = true;
        }

        if (h1p3Grabbed && !h1s3Filled && h1s3 != null && h1s3.hasSelection)
        {
            PlayAudioOnce(ref h1s3Filled, h1s3FilledAudio);
        }

        if (h1s3Filled && !h1p4Grabbed && IsGrabbed(h1p4))
        {
            PlayAudio(h1p4GrabAudio);
            PlayAudio(shrinkAudio);
            h1p4Grabbed = true;
        }

        if (h1p4Grabbed && !h1s4Filled && h1s4 != null && h1s4.hasSelection)
        {
            PlayAudioOnce(ref h1s4Filled, h1s4FilledAudio);
            PlayAudioOnce(ref hand2StartPlayed, hand2StartAudio);
        }
    }

    private void HandleHand2()
    {
        if (!h2s1Filled && h2s1 != null && h2s1.hasSelection)
            PlayAudioOnce(ref h2s1Filled, h2s1FilledAudio);

        if (h2s1Filled && !h2s2Filled && h2s2 != null && h2s2.hasSelection)
            PlayAudioOnce(ref h2s2Filled, h2s2FilledAudio);

        if (h2s2Filled && !h2s3Filled && h2s3 != null && h2s3.hasSelection)
            PlayAudioOnce(ref h2s3Filled, h2s3FilledAudio);

        if (h2s3Filled && !h2s4Filled && h2s4 != null && h2s4.hasSelection)
            PlayAudioOnce(ref h2s4Filled, h2s4FilledAudio);
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

    private void PlayAudioOnce(ref bool flag, AudioClip clip)
    {
        if (!flag && clip != null)
        {
            PlayAudio(clip);
            flag = true;
        }
    }
}
