using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DisableGrabAndMakeKinematicOnSocket : MonoBehaviour
{
    public XRSocketInteractor socket1Interactor;
    public XRSocketInteractor socket2Interactor;
    public float delayTime = 0.5f;

    private bool puzzleCompleted = false;
    private bool socket1Kinematic = false;
    private bool socket2Kinematic = false;

    void Update()
    {
        if (socket1Interactor.hasSelection)
        {
            var selectedInteractable = socket1Interactor.GetOldestInteractableSelected();
            if (selectedInteractable != null)
            {
                XRBaseInteractable baseInteractable = selectedInteractable as XRBaseInteractable;
                if (baseInteractable != null)
                {
                    StartCoroutine(HandleObjectAfterDelay(baseInteractable.gameObject, 1));
                }
            }
        }

        if (socket2Interactor.hasSelection)
        {
            var selectedInteractable = socket2Interactor.GetOldestInteractableSelected();
            if (selectedInteractable != null)
            {
                XRBaseInteractable baseInteractable = selectedInteractable as XRBaseInteractable;
                if (baseInteractable != null)
                {
                    StartCoroutine(HandleObjectAfterDelay(baseInteractable.gameObject, 2));
                }
            }
        }
    }

    private IEnumerator HandleObjectAfterDelay(GameObject obj, int socketNumber)
    {
        yield return new WaitForSeconds(delayTime);

        SetObjectProperties(obj, socketNumber);

        if (BothSocketsKinematic() && !puzzleCompleted)
        {
            puzzleCompleted = true;
        }
    }

    private void SetObjectProperties(GameObject obj, int socketNumber)
    {
        XRGrabInteractable grabInteractable = obj.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;

            if (socketNumber == 1)
            {
                socket1Kinematic = true;
            }
            else if (socketNumber == 2)
            {
                socket2Kinematic = true;
            }
        }
    }

    private bool BothSocketsKinematic()
    {
        return socket1Kinematic && socket2Kinematic;
    }

    public bool IsPuzzleCompleted()
    {
        return puzzleCompleted;
    }
}
