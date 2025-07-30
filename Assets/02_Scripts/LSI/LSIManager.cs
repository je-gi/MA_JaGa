using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class LSIManager : MonoBehaviour
{
    public event Action<string> OnLSIComplete;

    public AudioSource audioSource;
    public AudioClip lsiAudio;
    public GameObject[] lsiObjects;
    public GameObject cardManagerObject;
    public GameObject[] objectsToHideOnLSIComplete;
    public VisibilityCallout callout;
    public string calloutKey;
    public LearningTypeCalculator learningTypeCalculator;
    public CardManager cardManager;
    public XRSocketInteractor socketInteractor;
    public AudioClip socketSFX;
    public AudioClip postSocketAudio;
    public GameObject[] objectsToHideOnFirstPuzzleStart;

    private void OnEnable()
    {
        if (cardManager != null)
            cardManager.OnLSICompleted += CompleteLSI;
    }

    private void OnDisable()
    {
        if (cardManager != null)
            cardManager.OnLSICompleted -= CompleteLSI;
    }

    public void StartLSI()
    {
        if (cardManagerObject != null)
            cardManagerObject.SetActive(true);

        foreach (var obj in lsiObjects)
            if (obj != null)
                obj.SetActive(true);

        if (callout != null)
            callout.SetCalloutVisibility(calloutKey, true);
    }

    private void CompleteLSI(string learningType)
    {
        foreach (var obj in lsiObjects)
            if (obj != null)
                obj.SetActive(false);

        if (callout != null)
            callout.SetCalloutVisibility(calloutKey, false);

        if (learningTypeCalculator != null)
            learningTypeCalculator.ShowLearningTypeObject(learningType);

        if (objectsToHideOnLSIComplete != null)
            foreach (var obj in objectsToHideOnLSIComplete)
                if (obj != null)
                    obj.SetActive(false);

        StartCoroutine(PlayLSIAudioAndStartPuzzle(learningType));
    }

    private IEnumerator PlayLSIAudioAndStartPuzzle(string learningType)
    {
        if (audioSource != null && lsiAudio != null)
        {
            audioSource.clip = lsiAudio;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }

        if (socketInteractor != null)
            socketInteractor.enabled = true;

        yield return StartCoroutine(WaitForObjectAndPlayPrePuzzleAudio());

        if (objectsToHideOnFirstPuzzleStart != null)
            foreach (var obj in objectsToHideOnFirstPuzzleStart)
                if (obj != null)
                    obj.SetActive(false);

        OnLSIComplete?.Invoke(learningType);
    }

    private IEnumerator WaitForObjectAndPlayPrePuzzleAudio()
    {
        while (!socketInteractor.hasSelection)
            yield return null;

        if (audioSource != null)
        {
            if (socketSFX != null)
            {
                audioSource.clip = socketSFX;
                audioSource.Play();
                yield return new WaitWhile(() => audioSource.isPlaying);
            }

            if (postSocketAudio != null)
            {
                audioSource.clip = postSocketAudio;
                audioSource.Play();
                yield return new WaitWhile(() => audioSource.isPlaying);
            }
        }
    }
}
