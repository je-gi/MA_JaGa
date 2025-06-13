using System.Collections;
using UnityEngine;

public class RepairStationCompletion : MonoBehaviour
{
    public PanelStateController panelStateController;
    public GameObject canvasToHide;
    public GameObject bootsObject;
    public GameObject objectToHide;

    public AudioSource snailAudioSource;
    public AudioClip snailClip; 

    private bool completionTriggered = false;

    void Update()
    {
        if (!completionTriggered && AllPanelsCompleted())
        {
            StartCoroutine(HandleCompletion());
        }
    }

    private bool AllPanelsCompleted()
    {
        foreach (var button in panelStateController.levelButtons)
        {
            if (button.interactable)
                return false;
        }
        return true;
    }

    private IEnumerator HandleCompletion()
    {
        completionTriggered = true;

        if (canvasToHide != null)
            canvasToHide.SetActive(false);

        if (snailAudioSource != null && snailClip != null)
        {
            snailAudioSource.clip = snailClip;
            snailAudioSource.Play();
            yield return new WaitForSeconds(snailClip.length);
        }

        if (bootsObject != null)
            bootsObject.SetActive(true);

        if (objectToHide != null)
            objectToHide.SetActive(false);
    }

    public void TriggerCompletionManually()
    {
        if (!completionTriggered)
            StartCoroutine(HandleCompletion());
    }
}
