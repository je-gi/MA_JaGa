using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[System.Serializable]
public class SocketAudioPair
{
    public XRSocketInteractor socket;
    public AudioClip audioClip;
    [HideInInspector] public bool hasPlayed = false;
}

public class P2AudioManager : MonoBehaviour
{
    [Header("Allgemein")]
    public AudioSource audioSource;

    [Header("Einzelobjekt mit einmaligem Grab-Sound")]
    public GameObject singleGrabObject;
    public AudioClip singleGrabAudio;
    private bool singleGrabPlayed = false;

    [Header("Liste von Objekten (z.B. Dokumente)")]
    public List<GameObject> documentObjects = new List<GameObject>();
    public AudioClip firstDocumentGrabAudio;
    private bool documentGrabAudioPlayed = false;

    [Header("Trigger & Dokument-Trigger-Audio")]
    public TriggerNotifier triggerNotifier;
    public AudioClip firstTriggerAudio;
    public AudioClip secondTriggerAudio;
    private int documentsInTrigger = 0;
    private HashSet<GameObject> alreadyTriggered = new HashSet<GameObject>();

    [Header("Sockets mit Audio bei erster Befüllung")]
    public List<SocketAudioPair> socketAudioPairs = new List<SocketAudioPair>();

    [Header("Callout Steuerung")]
    public VisibilityCallout visibilityCallout;
    public XRSocketInteractor glassesSocket;

    [Header("Puzzle Manager Referenz")]
    public P2Manager puzzleManager;

    private bool callout3DObjectsShown = false;

    void Start()
    {
        if (triggerNotifier != null)
        {
            triggerNotifier.OnObjectEnteredTrigger.AddListener(HandleTriggerEnter);
        }
    }

    void Update()
    {
        if (!singleGrabPlayed && IsGrabbed(singleGrabObject))
        {
            PlayAudio(singleGrabAudio);
            singleGrabPlayed = true;
        }

        if (!documentGrabAudioPlayed)
        {
            foreach (var doc in documentObjects)
            {
                if (IsGrabbed(doc))
                {
                    PlayAudio(firstDocumentGrabAudio);
                    documentGrabAudioPlayed = true;

                    if (callout3DObjectsShown && visibilityCallout != null)
                    {
                        visibilityCallout.SetCalloutVisibility("3DObjectsCallout", false);
                        callout3DObjectsShown = false;
                    }

                    break;
                }
            }
        }

        if (glassesSocket != null)
        {
            if (glassesSocket.hasSelection && !callout3DObjectsShown)
            {
                visibilityCallout.SetCalloutVisibility("3DObjectsCallout", true);
                callout3DObjectsShown = true;
            }
            else if (callout3DObjectsShown && puzzleManager != null && puzzleManager.IsPuzzleCompleted)
            {
                visibilityCallout.SetCalloutVisibility("3DObjectsCallout", false);
                callout3DObjectsShown = false;
            }
        }

        foreach (var pair in socketAudioPairs)
        {
            if (pair.socket != null && pair.socket.hasSelection && !pair.hasPlayed)
            {
                PlayAudio(pair.audioClip);
                pair.hasPlayed = true;
            }
        }
    }

    private void HandleTriggerEnter(GameObject enteredObject)
    {
        if (documentObjects.Contains(enteredObject) && !alreadyTriggered.Contains(enteredObject))
        {
            alreadyTriggered.Add(enteredObject);
            documentsInTrigger++;

            if (documentsInTrigger == 1)
            {
                PlayAudio(firstTriggerAudio);
            }
            else if (documentsInTrigger == 2)
            {
                PlayAudio(secondTriggerAudio);
            }
        }
    }

    private bool IsGrabbed(GameObject obj)
    {
        if (obj == null) return false;

        XRGrabInteractable grab = obj.GetComponent<XRGrabInteractable>();
        return grab != null && grab.isSelected;
    }

    private void PlayAudio(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
