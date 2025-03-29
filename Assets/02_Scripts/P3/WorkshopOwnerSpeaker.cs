using UnityEngine;

public class WorkshopOwnerSpeaker : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip initialSpeech;
    public AudioClip afterGrabDusterSpeech;

    private bool hasSpokenAfterDusterGrab = false;

    private void Start()
    {
        PlaySpeech(initialSpeech);
    }

    public void OnDusterGrabbed()
    {
        if (!hasSpokenAfterDusterGrab)
        {
            hasSpokenAfterDusterGrab = true;
            PlaySpeech(afterGrabDusterSpeech);
        }
    }

    private void PlaySpeech(AudioClip clip)
    {
        if (clip == null || audioSource == null)
            return;

        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.clip = clip;
        audioSource.Play();
    }
}
