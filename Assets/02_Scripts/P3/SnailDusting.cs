using UnityEngine;

public class SnailDusting : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip wakeUpSpeech;
    private bool hasSpoken = false;

    public void WakeUp()
    {
        if (!hasSpoken)
        {
            audioSource.PlayOneShot(wakeUpSpeech);
            hasSpoken = true;
        }
    }
}