using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RepairStationCompletion : MonoBehaviour
{
    public PanelStateController panelStateController;
    public GameObject canvasToHide;
    public GameObject bootsObject;
    public AudioSource snailAudio;

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
        foreach (Button button in panelStateController.levelButtons)
        {
            if (button.interactable)
                return false;
        }
        return true;
    }

    private IEnumerator HandleCompletion()
    {
        completionTriggered = true;

        // Canvas ausblenden
        if (canvasToHide != null)
            canvasToHide.SetActive(false);

        // Schnecken-Audio abspielen
        if (snailAudio != null)
        {
            snailAudio.Play();
            yield return new WaitForSeconds(snailAudio.clip.length);
        }

        // Boots-Objekt einblenden
        if (bootsObject != null)
            bootsObject.SetActive(true);

        Debug.Log("Workbench abgeschlossen!");
    }
}
