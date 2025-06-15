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
        if (!hasSpoken)
        {
            audioSource.PlayOneShot(wakeUpSpeech);
            hasSpoken = true;
            Invoke(nameof(EnableSnailGrab), wakeUpSpeech.length);
        }
    }

    private void EnableSnailGrab()
    {
        snailGrabInteractable.enabled = true;
        snailRigidbody.isKinematic = false;

        if (objectToDisable != null) objectToDisable.SetActive(false);
        if (objectToEnable != null) objectToEnable.SetActive(true);
    }
}
