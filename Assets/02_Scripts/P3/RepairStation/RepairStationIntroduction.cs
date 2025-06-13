using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.UI;
using System.Collections;

public class RepairStationIntroduction : MonoBehaviour
{
    public static RepairStationIntroduction instance;

    public XRSocketInteractor snailSocket;
    public XRGrabInteractable snailGrabInteractable;
    public AudioSource audioSource;
    public AudioClip snailSpeech;

    public GameObject middleSymbol;
    public GameObject[] stationPanels;

    public float delayBeforeStart = 1f;
    public float panelInterval = 1f;

    private bool isSnailPlaced = false;
    private bool introInterrupted = false;
    private Coroutine introCoroutine;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        middleSymbol.SetActive(false);
        foreach (var panel in stationPanels)
            panel.SetActive(false);
    }

    private void Update()
    {
        if (!isSnailPlaced && snailSocket.hasSelection)
        {
            isSnailPlaced = true;

            if (audioSource != null && snailSpeech != null)
                audioSource.PlayOneShot(snailSpeech);

            Invoke(nameof(StartIntroSequence), delayBeforeStart);
        }
    }

    private void StartIntroSequence()
    {
        introCoroutine = StartCoroutine(ShowPanelsSequentially());
    }

    public void InterruptIntroAndHidePanels()
    {
        introInterrupted = true;

        if (introCoroutine != null)
            StopCoroutine(introCoroutine);

        middleSymbol.SetActive(false);
        foreach (var panel in stationPanels)
            panel.SetActive(false);
    }

    private IEnumerator ShowPanelsSequentially()
    {
        middleSymbol.SetActive(true);
        yield return new WaitForSeconds(panelInterval);

        foreach (GameObject panel in stationPanels)
        {
            if (introInterrupted) yield break;

            panel.SetActive(true);
            yield return new WaitForSeconds(panelInterval);
        }
    }
}
