using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DusterGrab : MonoBehaviour
{
    [Header("Audio beim ersten Grab")]
    public AudioSource audioSource;
    public AudioClip grabAudio;

    [Header("Snail & Abstaub-Einstellungen")]
    public SnailGrab snail;
    public float minMoveDistance = 0.1f;
    public int requiredDustings = 5;

    private XRGrabInteractable grabInteractable;
    private bool isBeingUsed = false;
    private int dustingCounter = 0;
    private bool isTouchingSnail = false;
    private Vector3 lastPosition;
    private bool grabSoundPlayed = false;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (!isBeingUsed)
        {
            isBeingUsed = true;
            dustingCounter = 0;
            lastPosition = transform.position;

            if (!grabSoundPlayed)
            {
                PlayGrabSound();
                grabSoundPlayed = true;
            }
        }
    }

    private void Update()
    {
        if (isBeingUsed && isTouchingSnail && DetectDustingMotion())
        {
            dustingCounter++;
            lastPosition = transform.position;

            if (dustingCounter >= requiredDustings)
            {
                snail.WakeUp();
                isBeingUsed = false;
            }
        }
    }

    private bool DetectDustingMotion()
    {
        return Vector3.Distance(transform.position, lastPosition) > minMoveDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Snail"))
        {
            isTouchingSnail = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Snail"))
        {
            isTouchingSnail = false;
        }
    }

    private void PlayGrabSound()
    {
        if (audioSource != null && grabAudio != null)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = grabAudio;
            audioSource.Play();
        }
    }
}
