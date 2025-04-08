using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DisableGrabAndMakeKinematicOnSocketSolo : MonoBehaviour
{
    public XRSocketInteractor socketInteractor;
    public float delayTime = 0.5f;

    void Update()
    {
        if (socketInteractor.hasSelection)
        {
            var selectedInteractable = socketInteractor.GetOldestInteractableSelected();
            if (selectedInteractable != null)
            {
                XRBaseInteractable baseInteractable = selectedInteractable as XRBaseInteractable;
                if (baseInteractable != null)
                {
                    StartCoroutine(HandleObjectAfterDelay(baseInteractable.gameObject));
                }
            }
        }
    }

    private IEnumerator HandleObjectAfterDelay(GameObject obj)
    {
        yield return new WaitForSeconds(delayTime);

        SetObjectProperties(obj);
    }

    private void SetObjectProperties(GameObject obj)
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
        }
    }
}
