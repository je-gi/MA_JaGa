using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections.Generic;

public class P1Manager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clipIntro;
    public AudioClip clipOnTriggerEntered;
    public AudioClip clipOnSuccess;
    public AudioClip clipOnFailure;
    public AudioClip clipOnPuzzleCompleted;
    public AudioClip clipOnObjectGrabbed;
    public AudioClip clipOnRevealObjects;

    [Header("Trigger Zone")]
    public Collider triggerZone;

    [Header("Camera (VR)")]
    public Transform vrCameraTransform;

    [Header("Socket Checker")]
    public SocketChecking socketChecker;

    [Header("Objects To Track")]
    public GameObject objectToTrack1;
    public GameObject objectToTrack2;

    [System.Serializable]
    public class SocketAudioPair
    {
        public XRSocketInteractor socket;
        public AudioClip audioClip;
        public bool hasPlayed = false;
    }

    [Header("Socket Audio Mapping")]
    public List<SocketAudioPair> socketAudioPairs;

    [Header("External Script")]
    public ShowObjectsWhenSocketsFilled showObjectsScript;

    [Header("DisableGrabAndMakeKinematic Script")]
    public DisableGrabAndMakeKinematicOnSocket disableGrabAndMakeKinematicOnSocket;

    [Header("Debug")]
    public bool activateManually = false;

    private bool hasStarted = false;
    private bool hasTriggeredAudioPlayed = false;
    private bool puzzleCompleted = false;
    private bool revealAudioPlayed = false;

    void Start()
    {
        foreach (var pair in socketAudioPairs)
        {
            pair.socket.selectEntered.AddListener((SelectEnterEventArgs args) => OnSocketFilled(pair));
        }

        if (objectToTrack1 != null)
        {
            XRGrabInteractable grab1 = objectToTrack1.GetComponent<XRGrabInteractable>();
            if (grab1 != null) grab1.selectEntered.AddListener(OnObjectGrabbed);
        }

        if (objectToTrack2 != null)
        {
            XRGrabInteractable grab2 = objectToTrack2.GetComponent<XRGrabInteractable>();
            if (grab2 != null) grab2.selectEntered.AddListener(OnObjectGrabbed);
        }

        socketChecker.OnPuzzleStatusChanged += OnPuzzleStatusChanged;
    }

    void Update()
    {
        if (activateManually && !hasStarted)
        {
            StartPuzzle();
            activateManually = false;
        }

        if (!hasTriggeredAudioPlayed && triggerZone != null && IsCameraInTrigger())
        {
            PlayClip(clipOnTriggerEntered);
            hasTriggeredAudioPlayed = true;
        }

        CheckPuzzleCompletion();
        CheckRevealObjectsFromExternalScript();
    }

    private void StartPuzzle()
    {
        if (hasStarted) return;

        hasStarted = true;

        if (clipIntro != null)
            PlayClip(clipIntro);
    }

    private void CheckPuzzleCompletion()
    {
        if (disableGrabAndMakeKinematicOnSocket != null && disableGrabAndMakeKinematicOnSocket.IsPuzzleCompleted() && !puzzleCompleted)
        {
            PlayClip(clipOnPuzzleCompleted);
            puzzleCompleted = true;
        }
    }

    private bool IsCameraInTrigger()
    {
        if (vrCameraTransform != null && triggerZone != null)
        {
            return triggerZone.bounds.Contains(vrCameraTransform.position);
        }
        return false;
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private void OnPuzzleStatusChanged(bool isCompleted)
    {
        if (isCompleted)
            PlayClip(clipOnSuccess);
        else
            PlayClip(clipOnFailure);
    }

    private void OnSocketFilled(SocketAudioPair pair)
    {
        if (!pair.hasPlayed && pair.audioClip != null)
        {
            PlayClip(pair.audioClip);
            pair.hasPlayed = true;
        }
    }

    private void OnObjectGrabbed(SelectEnterEventArgs args)
    {
        if (!hasTriggeredAudioPlayed && clipOnObjectGrabbed != null)
        {
            PlayClip(clipOnObjectGrabbed);
            hasTriggeredAudioPlayed = true;
        }
    }

    private void CheckRevealObjectsFromExternalScript()
    {
        if (showObjectsScript != null && showObjectsScript.AlreadyShown && !revealAudioPlayed)
        {
            PlayClip(clipOnRevealObjects);
            revealAudioPlayed = true;
        }
    }

    public bool IsPuzzleCompleted
    {
        get { return puzzleCompleted; }
    }

    public void StartPuzzleExternally()
    {
        StartPuzzle();
    }

    public bool IsCompletionAudioPlaying()
    {
        return audioSource.isPlaying;
    }
}
