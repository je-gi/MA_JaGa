using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class LayerChangeOnSocket : MonoBehaviour
{
    [Header("Socket und Layer")]
    [SerializeField] private XRSocketInteractor socketInteractor;
    [SerializeField] private InteractionLayerMask layerToAdd;

    [Header("Audio bei erfolgreicher Layeränderung")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip layerChangeAudio;

    [Header("Partikeleffekt bei Layeränderung")]
    [SerializeField] private ParticleSystem particleEffect;

    [Header("Callout Steuerung")]
    [SerializeField] private VisibilityCallout visibilityCallout;

    [Header("Puzzle Completion")]
    [SerializeField] private P2Manager puzzleManager;

    private bool calloutShown = false;

    public void ChangeLayerInSocket()
    {
        if (socketInteractor.hasSelection)
        {
            XRBaseInteractable selectedInteractable = (XRBaseInteractable)socketInteractor.GetOldestInteractableSelected();

            if (selectedInteractable != null)
            {
                GameObject selectedObject = selectedInteractable.gameObject;

                SetInteractionLayer(selectedObject);
            }
        }
        else
        {
            Debug.LogWarning("No object is currently in the socket.");
        }
    }

    private void SetInteractionLayer(GameObject objectInSocket)
    {
        if (objectInSocket != null)
        {
            XRGrabInteractable interactable = objectInSocket.GetComponent<XRGrabInteractable>();

            if (interactable != null)
            {
                InteractionLayerMask currentLayerMask = interactable.interactionLayers;
                InteractionLayerMask newLayerMask = AddInteractionLayer(currentLayerMask, layerToAdd);

                interactable.interactionLayers = newLayerMask;

                Debug.Log("Added interaction layer.");

                PlayAudio();
                PlayParticleEffect();
                ShowPrefabCallout();
            }
            else
            {
                Debug.LogError("XRGrabInteractable component not found on the object.");
            }
        }
    }

    private InteractionLayerMask AddInteractionLayer(InteractionLayerMask currentLayerMask, InteractionLayerMask layerToAdd)
    {
        return currentLayerMask | layerToAdd;
    }

    private void PlayAudio()
    {
        if (audioSource != null && layerChangeAudio != null)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = layerChangeAudio;
            audioSource.Play();
        }
    }

    private void PlayParticleEffect()
    {
        if (particleEffect != null)
        {
            particleEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            particleEffect.Play();
        }
    }

    private void ShowPrefabCallout()
    {
        if (visibilityCallout != null && !calloutShown)
        {
            visibilityCallout.SetCalloutVisibility("PrefabCallout", true);
            calloutShown = true;
        }
    }

    private void Update()
    {
        if (calloutShown && puzzleManager != null && puzzleManager.IsPuzzleCompleted)
        {
            if (visibilityCallout != null)
            {
                visibilityCallout.SetCalloutVisibility("PrefabCallout", false);
                calloutShown = false;
            }
        }
    }
}
