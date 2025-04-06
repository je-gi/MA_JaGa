using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PuzzleCompleteObjects : MonoBehaviour
{
    [Header("Sockets for Puzzle Completion")]
    public XRSocketInteractor socket1;   
    public XRSocketInteractor socket2; 

    [Header("Parts")]
    public GameObject Part01;  
    public GameObject Part02;  

    [Header("Settings")]
    public float delayTime = 0.5f;

    private bool puzzleCompleted = false;

    void Start()
    {
        SetObjectGrabbableAndKinematic(Part01, true);
        SetObjectGrabbableAndKinematic(Part02, true);
    }

    public void HandlePuzzleCompletion()
    {
        if (!puzzleCompleted && BothSocketsFilled())
        {
            puzzleCompleted = true;

            StartCoroutine(SetObjectsKinematicAfterDelay());
        }
    }

    private bool BothSocketsFilled()
    {
        return socket1.GetOldestInteractableSelected() != null && socket2.GetOldestInteractableSelected() != null;
    }

    private IEnumerator SetObjectsKinematicAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);

        SetObjectGrabbableAndKinematic(Part01, false);
        SetObjectGrabbableAndKinematic(Part02, false);
    }

    private void SetObjectGrabbableAndKinematic(GameObject obj, bool isGrabbable)
    {
        if (obj != null)
        {
            XRGrabInteractable grabInteractable = obj.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                grabInteractable.enabled = isGrabbable;
            }

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = !isGrabbable; 
            }
        }
    }
}
