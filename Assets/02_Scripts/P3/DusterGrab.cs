using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DusterGrab : MonoBehaviour
{
    public WorkshopOwnerSpeaker workshopOwner;
    public SnailDusting snail;

    [Header("Abstaub-Einstellungen")]
    public float minMoveDistance = 0.1f; 
    public int requiredDustings = 5; 

    private XRGrabInteractable grabInteractable;
    private bool isBeingUsed = false;
    private int dustingCounter = 0;
    private bool isTouchingSnail = false;
    private Vector3 lastPosition;

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
            workshopOwner.OnDusterGrabbed();
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
        return Mathf.Abs(transform.position.x - lastPosition.x) > minMoveDistance;
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
}
