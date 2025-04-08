using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GameManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip introAudioClip;
    public AudioClip lsiAudioClip;
    public AudioClip postSocketAudioClip;
    public AudioClip finalPuzzleCompletedAudioClip;

    public GameObject[] lsiObjects;
    public GameObject cardManagerObject;
    public LearningTypeCalculator learningTypeCalculator;
    public CardManager cardManager;

    public P1Manager p1;
    public P2Manager p2;
    public P3Manager p3;
    public P4Manager p4;

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

        firstPuzzleStarted = true;
        StartNextPuzzle();
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
            PlayFinalAudio();
        }
    }

    private void PlayFinalAudio()
    {
        if (finalPuzzleCompletedAudioClip != null && audioSource != null)
        {
            audioSource.clip = finalPuzzleCompletedAudioClip;
            audioSource.Play();
        }
    }

    private IEnumerator WaitForPuzzleCompletion(MonoBehaviour puzzleManager)
    {
        bool isCompleted = false;

        if (puzzleManager is P1Manager p1Manager)
        {
            while (!p1Manager.IsPuzzleCompleted)
            {
                yield return null;
            }
            isCompleted = true;
        }
        else if (puzzleManager is P2Manager p2Manager)
        {
            while (!p2Manager.IsPuzzleCompleted)
            {
                yield return null;
            }
            isCompleted = true;
        }
        else if (puzzleManager is P3Manager p3Manager)
        {
            while (!p3Manager.IsPuzzleCompleted)
            {
                yield return null;
            }
            isCompleted = true;
        }
        else if (puzzleManager is P4Manager p4Manager)
        {
            while (!p4Manager.IsPuzzleCompleted)
            {
                yield return null;
            }
            isCompleted = true;
        }

        if (isCompleted)
        {
            StartNextPuzzle();
        }
    }
}
