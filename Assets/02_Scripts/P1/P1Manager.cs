using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;
using System.Collections.Generic;

public class P1Manager : MonoBehaviour
{
    [Header("Trigger Zone")]
    public Collider triggerZone;
    public Transform PlayerCamera;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioSource loopAudioSource;
    public AudioClip clipIntro;
    public AudioClip clipOnTriggerEntered;
    public AudioClip clipOnSuccess;
    public AudioClip clipOnFailure;
    public AudioClip clipOnPuzzleCompleted;
    public AudioClip clipOnObjectGrabbed;
    public AudioClip clipOnRevealObjects;
    public AudioClip backgroundLoopClip;
    public float fadeOutDuration = 2f;

    [Header("Sockets")]
    public SocketChecking socketChecker;
    public GameObject objectToTrack1;
    public XRSocketInteractor socketToEnableAfterSuccess;
    public XRSocketInteractor headphoneSocket;

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
    public VisibilityCallout visibilityCallout;

    [Header("Timing")]
    public float introDelaySeconds = 2f;
    public float postIntroTriggerDelay = 1f;

    private bool hasStarted = false;
    private bool hasTriggeredAudioPlayed = false;
    private bool puzzleCompleted = false;
    private bool revealAudioPlayed = false;
    private bool canPlayTriggerAudio = false;
    private bool successClipFinished = false;

    [Header("Debug")]
    public bool activateManually = false;

    void Start()
    {
        foreach (var pair in socketAudioPairs)
        {
            SocketAudioPair currentPair = pair;
            currentPair.socket.selectEntered.AddListener((SelectEnterEventArgs args) => OnSocketFilled(currentPair));
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
            PlayClipWithCallback(clipOnTriggerEntered, OnTriggerClipFinished);
            hasTriggeredAudioPlayed = true;
        }

        CheckPuzzleCompletion();
        CheckRevealObjectsFromExternalScript();

        if (successClipFinished && headphoneSocket != null && headphoneSocket.GetOldestInteractableSelected() != null)
        {
            HideCallout("AudioListenerCallout");
        }
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

            if (loopAudioSource != null && loopAudioSource.isPlaying)
            {
                StartCoroutine(FadeOutLoopAudio());
            }

            if (showObjectsScript != null)
            {
                showObjectsScript.StopAnimation();
            }
        }
    }

    void CheckRevealObjectsFromExternalScript()
    {
        if (showObjectsScript != null && showObjectsScript.AlreadyShown && !revealAudioPlayed)
        {
            revealAudioPlayed = true;
            StartCoroutine(PlayRevealClipWithDelay());
        }
    }

    IEnumerator PlayRevealClipWithDelay()
    {
        yield return new WaitForSeconds(1f);

        if (clipOnRevealObjects != null && audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = clipOnRevealObjects;
            audioSource.Play();
        }
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

    void PlayClipWithCallback(AudioClip clip, System.Action onComplete)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
            StartCoroutine(WaitAndCallback(clip.length, onComplete));
        }
    }

    IEnumerator WaitAndCallback(float time, System.Action callback)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }

    IEnumerator FadeOutLoopAudio()
    {
        float startVolume = loopAudioSource.volume;
        float elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            loopAudioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeOutDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        loopAudioSource.Stop();
        loopAudioSource.volume = startVolume;
    }

    void OnPuzzleStatusChanged(bool isCompleted)
    {
        if (isCompleted)
        {
            PlayClipWithCallback(clipOnSuccess, OnSuccessClipFinished);

            if (loopAudioSource != null && backgroundLoopClip != null)
            {
                loopAudioSource.clip = backgroundLoopClip;
                loopAudioSource.loop = true;
                loopAudioSource.Play();
            }

            if (objectToTrack1 != null) objectToTrack1.SetActive(true);
            if (socketToEnableAfterSuccess != null) socketToEnableAfterSuccess.gameObject.SetActive(true);

            HideCallout("AudioClipCallout");
            HideCallout("AudioSourceCallout");
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
            pair.socket.GetOldestInteractableSelected() != null &&
            pair.socket.GetOldestInteractableSelected().transform == objectToTrack1.transform)
        {
            if (showObjectsScript != null && !showObjectsScript.AlreadyShown)
            {
                showObjectsScript.StartSequence();
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

    bool IsCameraInTrigger()
    {
        if (PlayerCamera != null && triggerZone != null)
        {
            return triggerZone.bounds.Contains(PlayerCamera.position);
        }
        return false;
    }

    void ShowCallout(string key)
    {
        if (visibilityCallout != null)
            visibilityCallout.SetCalloutVisibility(key, true);
    }

    void HideCallout(string key)
    {
        if (visibilityCallout != null)
            visibilityCallout.SetCalloutVisibility(key, false);
    }

    void OnTriggerClipFinished()
    {
        ShowCallout("AudioClipCallout");
        ShowCallout("AudioSourceCallout");
    }

    void OnSuccessClipFinished()
    {
        successClipFinished = true;
        ShowCallout("AudioListenerCallout");
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
