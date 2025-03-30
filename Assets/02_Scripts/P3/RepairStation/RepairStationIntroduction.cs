using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class RepairStationIntroduction : MonoBehaviour
{
    public XRSocketInteractor snailSocket;
    public XRGrabInteractable snailGrabInteractable;
    public AudioSource audioSource;
    public AudioClip snailSpeech;
    public Canvas canvas;
    public GameObject[] panels;

    public float canvasFadeInTime = 2f;
    public float panelInterval = 1f;

    private bool isSnailPlaced = false;
    private int currentPanelIndex = 0;

    private void Start()
    {
        canvas.enabled = false;
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isSnailPlaced && snailSocket.hasSelection)
        {
            isSnailPlaced = true;
            audioSource.PlayOneShot(snailSpeech);
            Invoke(nameof(ShowCanvas), canvasFadeInTime);
        }
    }

    private void ShowCanvas()
    {
        canvas.enabled = true;
        InvokeRepeating(nameof(ShowPanelsSequentially), 0, panelInterval);
    }

    private void ShowPanelsSequentially()
    {
        if (currentPanelIndex < panels.Length)
        {
            panels[currentPanelIndex].SetActive(true);
            currentPanelIndex++;
        }
        else
        {
            CancelInvoke(nameof(ShowPanelsSequentially));
        }
    }
}
