using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PrefabDuplicator : MonoBehaviour
{
    [Header("Sockets & Prefabs")]
    public XRSocketInteractor eyeSocket;
    public XRSocketInteractor cardSocket;
    public Transform spawnPoint;

    public GameObject greenEyePrefab;
    public GameObject greenEyeAnimatedPrefab;

    [Header("Zusatzobjekt bei Karte im Socket")]
    public GameObject objectToShowWhenCardPresent;

    [Header("Audio")]
    public AudioSource sfxAudioSource;
    public AudioClip successSound;
    public AudioClip errorSound;

    public AudioSource eventAudioSource;
    public AudioClip eyeSocketInsertedAudio;
    public AudioClip normalEyeSpawnAudio;
    public AudioClip animatedEyeSpawnAudio;

    [Header("Partikeleffekt beim Spawn")]
    public ParticleSystem spawnParticleEffect;

    [Header("Callouts")]
    public VisibilityCallout visibilityCallout;
    private bool animationCalloutShown = false;

    [Header("Puzzle")]
    public P2Manager p2Manager;

    private bool hasEyeSocketBeenFilled = false;
    private bool hasNormalEyeSpawned = false;
    private bool hasAnimatedEyeSpawned = false;

    private void Update()
    {
        if (objectToShowWhenCardPresent != null)
        {
            objectToShowWhenCardPresent.SetActive(cardSocket.hasSelection);
        }

        if (eyeSocket.hasSelection && !hasEyeSocketBeenFilled)
        {
            PlaySound(eventAudioSource, eyeSocketInsertedAudio);
            hasEyeSocketBeenFilled = true;
        }

        if (animationCalloutShown && p2Manager != null && p2Manager.IsPuzzleCompleted)
        {
            visibilityCallout.SetCalloutVisibility("AnimationCallout", false);
            animationCalloutShown = false;
        }
    }

    public void OnButtonPressed()
    {
        if (!eyeSocket.hasSelection)
        {
            PlaySound(sfxAudioSource, errorSound);
            return;
        }

        bool isAnimated = cardSocket.hasSelection;
        GameObject prefabToSpawn = isAnimated ? greenEyeAnimatedPrefab : greenEyePrefab;
        GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

        PlaySound(sfxAudioSource, successSound);
        PlaySpawnEffect();

        if (isAnimated && !hasAnimatedEyeSpawned)
        {
            PlaySound(eventAudioSource, animatedEyeSpawnAudio);
            hasAnimatedEyeSpawned = true;
        }
        else if (!isAnimated && !hasNormalEyeSpawned)
        {
            PlaySound(eventAudioSource, normalEyeSpawnAudio);
            hasNormalEyeSpawned = true;
        }

        if (TryGetComponent<GlassesShow>(out var glassesShow))
        {
            glassesShow.RegisterEyeObject(spawnedObject);
        }
    }

    private void PlaySound(AudioSource source, AudioClip clip)
    {
        if (source != null && clip != null)
        {
            source.PlayOneShot(clip);
        }
    }

    private void PlaySpawnEffect()
    {
        if (spawnParticleEffect != null)
        {
            spawnParticleEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            spawnParticleEffect.Play();
        }
    }
}
