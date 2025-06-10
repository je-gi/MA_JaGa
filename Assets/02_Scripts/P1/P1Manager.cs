using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;
using System.Collections.Generic;

public class P1Manager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clipIntro;
    public AudioClip clipOnTriggerEntered;
    public AudioClip clipOnSuccess;
    public AudioClip clipOnFailure;
    public AudioClip clipOnPuzzleCompleted;
    public AudioClip clipOnObjectGrabbed;
    public AudioClip clipOnRevealObjects;

    public Collider triggerZone;
    public Transform vrCameraTransform;

    public SocketChecking socketChecker;
    public GameObject objectToTrack1;
    public XRSocketInteractor socketToEnableAfterSuccess;

    [System.Serializable]
    public class SocketAudioPair
    {
        public XRSocketInteractor socket;
        public AudioClip audioClip;
        public bool hasPlayed = false;
    }

    public List<SocketAudioPair> socketAudioPairs;
    public ShowObjectsWhenSocketsFilled showObjectsScript;
    public DisableGrabAndMakeKinematicOnSocket disableGrabAndMakeKinematicOnSocket;

    public bool activateManually = false;

    [Header("Timing")]
    public float introDelaySeconds = 2f;
    public float postIntroTriggerDelay = 1f;

    private bool hasStarted = false;
    private bool hasTriggeredAudioPlayed = false;
    private bool puzzleCompleted = false;
    private bool revealAudioPlayed = false;
    private bool canPlayTriggerAudio = false;

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

        if (socketToEnableAfterSuccess != null)
        {
            socketToEnableAfterSuccess.gameObject.SetActive(false);
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

        if (hasStarted && canPlayTriggerAudio && !hasTriggeredAudioPlayed && triggerZone != null && IsCameraInTrigger())
        {
            PlayClip(clipOnTriggerEntered);
            hasTriggeredAudioPlayed = true;
        }

        CheckPuzzleCompletion();
        CheckRevealObjectsFromExternalScript();
    }

    void StartPuzzle()
    {
        if (hasStarted) return;
        hasStarted = true;
        StartCoroutine(PlayIntroWithDelay());
    }

    IEnumerator PlayIntroWithDelay()
    {
        yield return new WaitForSeconds(introDelaySeconds);
        if (clipIntro != null)
        {
            audioSource.clip = clipIntro;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }

        yield return new WaitForSeconds(postIntroTriggerDelay);
        canPlayTriggerAudio = true;
    }

    void CheckPuzzleCompletion()
    {
        if (disableGrabAndMakeKinematicOnSocket != null &&
            disableGrabAndMakeKinematicOnSocket.IsPuzzleCompleted() &&
            !puzzleCompleted)
        {
            PlayClip(clipOnPuzzleCompleted);
            puzzleCompleted = true;

            if (showObjectsScript != null)
            {
                showObjectsScript.StopAnimation();
            }
        }
    }

    bool IsCameraInTrigger()
    {
        if (vrCameraTransform != null && triggerZone != null)
        {
            return triggerZone.bounds.Contains(vrCameraTransform.position);
        }
        return false;
    }

    void PlayClip(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    void OnPuzzleStatusChanged(bool isCompleted)
    {
        if (isCompleted)
        {
            PlayClip(clipOnSuccess);
            if (objectToTrack1 != null) objectToTrack1.SetActive(true);
            if (socketToEnableAfterSuccess != null) socketToEnableAfterSuccess.gameObject.SetActive(true);
        }
        else
        {
            PlayClip(clipOnFailure);
        }
    }

    void OnSocketFilled(SocketAudioPair pair)
    {
        if (!pair.hasPlayed && pair.audioClip != null)
        {
            PlayClip(pair.audioClip);
            pair.hasPlayed = true;
        }

        if (objectToTrack1 != null &&
            pair.socket.firstInteractableSelected != null &&
            pair.socket.firstInteractableSelected.transform == objectToTrack1.transform)
        {
            if (showObjectsScript != null && !showObjectsScript.AlreadyShown)
            {
                showObjectsScript.ShowAndAnimate();
            }
        }
    }

    void OnObjectGrabbed(SelectEnterEventArgs args)
    {
        if (!hasTriggeredAudioPlayed && clipOnObjectGrabbed != null && canPlayTriggerAudio)
        {
            PlayClip(clipOnObjectGrabbed);
            hasTriggeredAudioPlayed = true;
        }
    }

    void CheckRevealObjectsFromExternalScript()
    {
        if (showObjectsScript != null && showObjectsScript.AlreadyShown && !revealAudioPlayed)
        {
            PlayClip(clipOnRevealObjects);
            revealAudioPlayed = true;
        }
    }

    public bool IsPuzzleCompleted => puzzleCompleted;

    public void StartPuzzleExternally()
    {
        StartPuzzle();
    }

    public bool IsCompletionAudioPlaying()
    {
        return audioSource.isPlaying;
    }
}
