using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    public P1Manager p1;
    public P2Manager p2;
    public P3Manager p3;
    public P4Manager p4;
    public AudioSource finalAudioSource;
    public AudioClip finalPuzzleCompletedAudioClip;
    public IntroManager introManager;

    [Header("Debug")]
    public bool forceCompleteGame = false;

    private bool hasForcedCompletion = false;
    private Queue<MonoBehaviour> puzzleQueue = new Queue<MonoBehaviour>();

    private void Update()
    {
        if (forceCompleteGame && !hasForcedCompletion)
        {
            forceCompleteGame = false;
            hasForcedCompletion = true;
            StartCoroutine(PlayFinalAudioAndEnd());
        }
    }

    public void StartPuzzleFlow(string learningType)
    {
        SetPuzzleOrder(learningType);
        StartNextPuzzle();
    }

    private void SetPuzzleOrder(string learningType)
    {
        puzzleQueue.Clear();

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

    private IEnumerator PlayFinalAudioAndEnd()
    {
        if (finalPuzzleCompletedAudioClip != null && finalAudioSource != null)
        {
            finalAudioSource.clip = finalPuzzleCompletedAudioClip;
            finalAudioSource.Play();
            yield return new WaitWhile(() => finalAudioSource.isPlaying);
        }

        if (introManager != null && introManager.fadeScreen != null)
        {
            introManager.fadeScreen.FadeOut();
            yield return new WaitForSeconds(introManager.fadeScreen.fadeOutDuration);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
