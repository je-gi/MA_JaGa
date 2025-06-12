using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using TMPro;

public class CardManager : MonoBehaviour
{
    public XRSocketInteractor[] sockets;
    public GameObject[] setObjects;
    public GameObject nextButton;

    public TMP_Text infoText;
    public TMP_Text secondaryInfoText;

    public string[] setDescriptions;
    public string[] secondarySetDescriptions;

    public AudioClip successClip;
    public AudioClip failClip;

    public AudioSource audioSource;

    public event System.Action<string> OnLSICompleted;

    private int currentSetIndex = 0;
    private LearningTypeCalculator learningTypeCalculator;

    void Start()
    {
        learningTypeCalculator = Object.FindFirstObjectByType<LearningTypeCalculator>();
        SpawnSet(currentSetIndex);
    }

    void SpawnSet(int setIndex)
    {
        foreach (var setObject in setObjects)
        {
            setObject.SetActive(false);
        }

        if (setIndex < setObjects.Length)
        {
            setObjects[setIndex].SetActive(true);

            if (setIndex < setDescriptions.Length)
            {
                infoText.text = setDescriptions[setIndex];
            }
            else
            {
                infoText.text = "";
            }

            if (secondaryInfoText != null)
            {
                if (setIndex < secondarySetDescriptions.Length)
                {
                    secondaryInfoText.text = secondarySetDescriptions[setIndex];
                }
                else
                {
                    secondaryInfoText.text = "";
                }
            }
        }
    }

    public void NextSet()
    {
        if (AreAllSocketsFilled())
        {
            if (audioSource != null && successClip != null)
                audioSource.PlayOneShot(successClip);

            learningTypeCalculator.AddScores(sockets);
            RemoveCardsFromSockets();
            currentSetIndex++;

            if (currentSetIndex < setObjects.Length)
            {
                SpawnSet(currentSetIndex);
            }
            else
            {
                string finalLearningType = learningTypeCalculator.CalculateFinalLearningType();
                OnLSICompleted?.Invoke(finalLearningType);
            }
        }
        else
        {
            if (audioSource != null && failClip != null)
                audioSource.PlayOneShot(failClip);
        }
    }

    bool AreAllSocketsFilled()
    {
        foreach (var socket in sockets)
        {
            IXRSelectInteractable obj = socket.GetOldestInteractableSelected();
            if (obj == null)
            {
                return false;
            }
        }
        return true;
    }

    void RemoveCardsFromSockets()
    {
        foreach (var socket in sockets)
        {
            IXRSelectInteractable interactable = socket.GetOldestInteractableSelected();
            if (interactable != null)
            {
                Destroy(interactable.transform.gameObject);
            }
        }
    }
}
