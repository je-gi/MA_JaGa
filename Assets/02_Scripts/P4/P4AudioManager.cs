using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class P4AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Hand 1 Sockets")]
    public XRSocketInteractor h1s1;
    public XRSocketInteractor h1s2;
    public XRSocketInteractor h1s3;

    [Header("Hand 2 Sockets")]
    public XRSocketInteractor h2s1;
    public XRSocketInteractor h2s2;
    public XRSocketInteractor h2s3;

    [Header("Audio Clips")] 
    public AudioClip h1s1FilledAudio;
    public AudioClip h1s2FilledAudio;
    public AudioClip h1s3FilledAudio;
    public AudioClip h1SpawnAudio1;
    public AudioClip h1SpawnAudio2;
    public AudioClip h2s1FilledAudio;
    public AudioClip h2s2FilledAudio;
    public AudioClip h2s3FilledAudio;
    public AudioClip h2SpawnAudio;

    private bool h1s1Played = false;
    private bool h1s2Played = false;
    private bool h1s3Played = false;
    private bool h1Spawned = false;
    private bool h2s1Played = false;
    private bool h2s2Played = false;
    private bool h2s3Played = false;
    private bool h2Spawned = false;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            if (!h1s1Played && h1s1.hasSelection)
            {
                PlayAudio(h1s1FilledAudio);
                h1s1Played = true;
            }
            else if (!h1s2Played && h1s2.hasSelection)
            {
                PlayAudio(h1s2FilledAudio);
                h1s2Played = true;
            }
            else if (!h1s3Played && h1s3.hasSelection)
            {
                PlayAudio(h1s3FilledAudio);
                h1s3Played = true;
            }
            else if (!h1Spawned && h1s1Played && h1s2Played && h1s3Played)
            {
                StartCoroutine(PlaySequential(h1SpawnAudio1, h1SpawnAudio2));
                h1Spawned = true;
            }
            else if (!h2s1Played && h2s1.hasSelection)
            {
                PlayAudio(h2s1FilledAudio);
                h2s1Played = true;
            }
            else if (!h2s2Played && h2s2.hasSelection)
            {
                PlayAudio(h2s2FilledAudio);
                h2s2Played = true;
            }
            else if (!h2s3Played && h2s3.hasSelection)
            {
                PlayAudio(h2s3FilledAudio);
                h2s3Played = true;
            }
            else if (!h2Spawned && h2s1Played && h2s2Played && h2s3Played)
            {
                PlayAudio(h2SpawnAudio);
                h2Spawned = true;
            }
        }
    }

    private void PlayAudio(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private System.Collections.IEnumerator PlaySequential(AudioClip first, AudioClip second)
    {
        PlayAudio(first);
        yield return new WaitForSeconds(first.length);
        PlayAudio(second);
    }
}
