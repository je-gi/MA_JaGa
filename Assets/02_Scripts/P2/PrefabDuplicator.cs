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

    public AudioSource firstSpawnAudioSource;    
    public AudioClip firstSpawnAudio;

    [Header("Partikeleffekt beim Spawn")]
    public ParticleSystem spawnParticleEffect;

    [Header("Callouts")]
    public VisibilityCallout visibilityCallout;
    private bool animationCalloutShown = false;

    [Header("Puzzle")]
    public P2Manager p2Manager;

    private bool hasSpawnedOnce = false;

    private void Update()
    {
        if (objectToShowWhenCardPresent != null)
        {
            objectToShowWhenCardPresent.SetActive(cardSocket.hasSelection);
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

        GameObject prefabToSpawn = cardSocket.hasSelection ? greenEyeAnimatedPrefab : greenEyePrefab;
        GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

        PlaySound(sfxAudioSource, successSound);
        PlaySpawnEffect();

        if (!hasSpawnedOnce)
        {
            PlaySound(firstSpawnAudioSource, firstSpawnAudio);

            if (visibilityCallout != null)
            {
                visibilityCallout.SetCalloutVisibility("AnimationCallout", true);
                animationCalloutShown = true;
            }

            hasSpawnedOnce = true;
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
