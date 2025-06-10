using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ShowObjectsWhenSocketsFilled : MonoBehaviour
{
    public XRSocketInteractor triggerSocket;

    public List<GameObject> objectsToShow;
    public List<GameObject> objectsToHide;

    public Animator loopingAnimator;
    public string startTrigger = "HeadphonesOn";
    public string stopTrigger = "StopSpin";

    private bool alreadyShown = false;
    public bool AlreadyShown => alreadyShown;

    void Update()
    {
        if (!alreadyShown && triggerSocket.GetOldestInteractableSelected() != null)
        {
            ShowAndAnimate();
        }
    }

    public void ShowAndAnimate()
    {
        if (alreadyShown) return;

        foreach (var go in objectsToShow)
            if (go != null) go.SetActive(true);

        foreach (var go in objectsToHide)
            if (go != null) go.SetActive(false);

        if (loopingAnimator != null)
        {
            loopingAnimator.SetTrigger(startTrigger);
        }

        alreadyShown = true;
    }

    public void StopAnimation()
    {
        if (loopingAnimator != null)
        {
            loopingAnimator.SetTrigger(stopTrigger);
        }
    }
}
