using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SnailGrab : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip wakeUpSpeech;
    public XRGrabInteractable snailGrabInteractable;
    public Rigidbody snailRigidbody;
    public bool HasSpoken => hasSpoken;

    [Header("GameObjects To Switch")]
    public GameObject objectToDisable;
    public GameObject objectToEnable;

    private bool hasSpoken = false;

    private void Awake()
    {
        snailGrabInteractable.enabled = false;
        snailRigidbody.isKinematic = true;
    }

    public void WakeUp()
    {
        if (hasSpoken) return;
        hasSpoken = true;

        if (objectToDisable != null) objectToDisable.SetActive(false);
        if (objectToEnable != null) objectToEnable.SetActive(true);

    
        Invoke(nameof(PlayWakeUpAudio), 1f);

    
        if (wakeUpSpeech != null)
        {
            float totalDelay = 1f + wakeUpSpeech.length;
            Invoke(nameof(EnableSnailGrab), totalDelay);
        }
        else
        {
            EnableSnailGrab(); 
        }
    }

    private void PlayWakeUpAudio()
    {
        if (audioSource != null && wakeUpSpeech != null)
        {
            audioSource.PlayOneShot(wakeUpSpeech);
        }
    }

    private void EnableSnailGrab()
    {
        snailGrabInteractable.enabled = true;
        snailRigidbody.isKinematic = false;
    }
}
