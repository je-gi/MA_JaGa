using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PrefabDuplicator : MonoBehaviour
{
    [Header("Sockets & Prefabs")]
    public XRSocketInteractor mainSocketInteractor;
    public XRSocketInteractor variantSocketInteractor;
    public Transform spawnPoint;
    public GameObject defaultPrefab;
    public GameObject variantPrefab1;
    public GameObject variantPrefab2;
    public GameObject variantPrefab3;
    public GameObject variantPrefab4;

    private GameObject selectedPrefab;

    [Header("Optional: GlassesShow (falls benötigt)")]
    public GlassesShow glassesShow;

    [Header("Audio beim ersten Spawn")]
    public AudioSource audioSource;
    public AudioClip firstSpawnAudio;
    private bool hasSpawnedOnce = false;

    private void Start()
    {
        selectedPrefab = defaultPrefab;
    }

    public void OnButtonPressed()
    {
        if (mainSocketInteractor.hasSelection)
        {
            if (variantSocketInteractor.hasSelection)
            {
                var selectedInteractable = variantSocketInteractor.GetOldestInteractableSelected();

                if (selectedInteractable != null)
                {
                    GameObject variantObject = selectedInteractable.transform.gameObject;

                    if (variantObject.CompareTag("Variant01"))
                    {
                        selectedPrefab = variantPrefab1;
                    }
                    else if (variantObject.CompareTag("Variant02"))
                    {
                        selectedPrefab = variantPrefab2;
                    }
                    else if (variantObject.CompareTag("Variant03"))
                    {
                        selectedPrefab = variantPrefab3;
                    }
                    else if (variantObject.CompareTag("Variant04"))
                    {
                        selectedPrefab = variantPrefab4;
                    }
                }
            }
            else
            {
                selectedPrefab = defaultPrefab;
            }

            GameObject spawnedObject = Instantiate(selectedPrefab, spawnPoint.position, spawnPoint.rotation);

            if (!hasSpawnedOnce)
            {
                PlayAudio();
                hasSpawnedOnce = true;
            }

            if (glassesShow != null)
            {
                glassesShow.RegisterEyeObject(spawnedObject);
            }
        }
    }

    private void PlayAudio()
    {
        if (audioSource != null && firstSpawnAudio != null)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = firstSpawnAudio;
            audioSource.Play();
        }
    }
}
