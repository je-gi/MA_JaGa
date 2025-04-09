using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GameManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip introAudioClip;
    public AudioClip lsiAudioClip;
    public AudioClip postSocketAudioClip;
    public AudioClip finalPuzzleCompletedAudioClip;

    [Header("LSI Objekte")]
    public GameObject[] lsiObjects;
    public GameObject cardManagerObject;

    [Header("Objekte, die nach LSI ausgeblendet werden sollen")]
    public GameObject[] objectsToHideOnFirstPuzzleStart;

    [Header("LSI-Komponenten")]
    public LearningTypeCalculator learningTypeCalculator;
    public CardManager cardManager;

    [Header("Puzzle-Manager")]
    public P1Manager p1;
    public P2Manager p2;
    public P3Manager p3;
    public P4Manager p4;

    [Header("XR Interaktionen")]
    public XRSocketInteractor socketInteractor;

    private bool firstPuzzleStarted = false;
    private Queue<MonoBehaviour> puzzleQueue = new Queue<MonoBehaviour>();

    private void Start()
    {
        foreach (var obj in lsiObjects)
        {
            obj.SetActive(false);
        }
        cardManagerObject.SetActive(false);

        StartCoroutine(StartIntroSequence());

        cardManager.OnLSICompleted += OnLSIComplete;
    }

    private IEnumerator StartIntroSequence()
    {
        audioSource.clip = introAudioClip;
        audioSource.Play();
        yield return new WaitForSeconds(introAudioClip.length);

        cardManagerObject.SetActive(true);
        foreach (var obj in lsiObjects)
        {
            obj.SetActive(true);
        }
    }

    private void OnLSIComplete(string learningType)
    {
        audioSource.clip = lsiAudioClip;
        audioSource.Play();

        learningTypeCalculator.ShowLearningTypeObject(learningType);
        SetPuzzleOrder(learningType);

        if (!firstPuzzleStarted)
        {
            StartCoroutine(WaitForObjectInSocketAndStartPuzzle());
        }
    }

    private IEnumerator WaitForObjectInSocketAndStartPuzzle()
    {
        while (!socketInteractor.hasSelection)
        {
            yield return null;
        }

        audioSource.clip = postSocketAudioClip;
        audioSource.Play();

        yield return new WaitWhile(() => audioSource.isPlaying);

        HideInitialObjects();

        firstPuzzleStarted = true;
        StartNextPuzzle();
    }

    private void HideInitialObjects()
    {
        if (objectsToHideOnFirstPuzzleStart != null)
        {
            foreach (var obj in objectsToHideOnFirstPuzzleStart)
            {
                if (obj != null)
                    obj.SetActive(false);
            }
        }
    }

    private void SetPuzzleOrder(string learningType)
    {
        if (learningType == "Diverging")
        {
            puzzleQueue.Enqueue(p1);
            puzzleQueue.Enqueue(p2);
            puzzleQueue.Enqueue(p3);
            puzzleQueue.Enqueue(p4);
        }
        else if (learningType == "Assimilating")
        {
            puzzleQueue.Enqueue(p2);
            puzzleQueue.Enqueue(p3);
            puzzleQueue.Enqueue(p4);
            puzzleQueue.Enqueue(p1);
        }
        else if (learningType == "Converging")
        {
            puzzleQueue.Enqueue(p3);
            puzzleQueue.Enqueue(p4);
            puzzleQueue.Enqueue(p1);
            puzzleQueue.Enqueue(p2);
        }
        else if (learningType == "Accommodating")
        {
            puzzleQueue.Enqueue(p4);
            puzzleQueue.Enqueue(p1);
            puzzleQueue.Enqueue(p2);
            puzzleQueue.Enqueue(p3);
        }
    }

    private void StartNextPuzzle()
    {
        if (puzzleQueue.Count > 0)
        {
            MonoBehaviour currentPuzzle = puzzleQueue.Dequeue();

            if (currentPuzzle is P1Manager p1Manager)
            {
                p1Manager.StartPuzzleExternally();
                StartCoroutine(WaitForPuzzleCompletion(p1Manager));
            }
            else if (currentPuzzle is P2Manager p2Manager)
            {
                p2Manager.StartPuzzleExternally();
                StartCoroutine(WaitForPuzzleCompletion(p2Manager));
            }
            else if (currentPuzzle is P3Manager p3Manager)
            {
                p3Manager.StartPuzzleExternally();
                StartCoroutine(WaitForPuzzleCompletion(p3Manager));
            }
            else if (currentPuzzle is P4Manager p4Manager)
            {
                p4Manager.StartPuzzleExternally();
                StartCoroutine(WaitForPuzzleCompletion(p4Manager));
            }
        }
        else
        {
            StartCoroutine(PlayFinalAudioAndEnd());
        }
    }

    private IEnumerator PlayFinalAudioAndEnd()
    {
        if (finalPuzzleCompletedAudioClip != null && audioSource != null)
        {
            audioSource.clip = finalPuzzleCompletedAudioClip;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }

        Debug.Log("Alle Puzzles abgeschlossen.");
    }

    private IEnumerator WaitForPuzzleCompletion(MonoBehaviour puzzleManager)
    {
        if (puzzleManager is P1Manager p1Manager)
            yield return new WaitUntil(() => p1Manager.IsPuzzleCompleted);
        else if (puzzleManager is P2Manager p2Manager)
            yield return new WaitUntil(() => p2Manager.IsPuzzleCompleted);
        else if (puzzleManager is P3Manager p3Manager)
            yield return new WaitUntil(() => p3Manager.IsPuzzleCompleted);
        else if (puzzleManager is P4Manager p4Manager)
            yield return new WaitUntil(() => p4Manager.IsPuzzleCompleted);

        if (puzzleManager is P1Manager p1m && p1m.audioSource != null)
            yield return new WaitWhile(() => p1m.IsCompletionAudioPlaying());
        else if (puzzleManager is P2Manager p2m && p2m.audioSource != null)
            yield return new WaitWhile(() => p2m.IsCompletionAudioPlaying());
        else if (puzzleManager is P3Manager p3m && p3m.audioSource != null)
            yield return new WaitWhile(() => p3m.IsCompletionAudioPlaying());
        else if (puzzleManager is P4Manager p4m && p4m.audioSource != null)
            yield return new WaitWhile(() => p4m.IsCompletionAudioPlaying());

        StartNextPuzzle();
    }
}
