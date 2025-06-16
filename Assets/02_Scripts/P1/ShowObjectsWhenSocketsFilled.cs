using UnityEngine;
using System.Collections;
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

    public GameObject objectToHideAfterDelay;
    public List<GameObject> objectsToShowAfterDelay;

    public AudioSource audioSource;
    public AudioClip showSound;

    public ParticleSystem particleEffect;

    private bool alreadyShown = false;
    public bool AlreadyShown => alreadyShown;

    void Update()
    {
        if (!alreadyShown && triggerSocket.GetOldestInteractableSelected() != null)
        {
            StartSequence();
        }
    }

    public void StartSequence()
    {
        if (alreadyShown) return;
        alreadyShown = true;
        StartCoroutine(ShowSequence());
    }

    private IEnumerator ShowSequence()
    {
        if (loopingAnimator != null)
        {
            loopingAnimator.SetTrigger(startTrigger);
        }

        yield return new WaitForSeconds(1f);

        foreach (var go in objectsToHide)
        {
            if (go != null) go.SetActive(false);
        }

        foreach (var go in objectsToShow)
        {
            if (go != null) go.SetActive(true);
        }

        if (objectToHideAfterDelay != null)
        {
            objectToHideAfterDelay.SetActive(false);
        }

        foreach (var go in objectsToShowAfterDelay)
        {
            if (go != null) go.SetActive(true);
        }

        if (audioSource != null && showSound != null)
        {
            audioSource.PlayOneShot(showSound);
        }

        if (particleEffect != null)
        {
            particleEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            particleEffect.Play();
        }
    }

    public void StopAnimation()
    {
        if (loopingAnimator != null)
        {
            loopingAnimator.SetTrigger(stopTrigger);
        }
    }
}
