using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RepairStationCompletion : MonoBehaviour
{
    public PanelStateController panelStateController;
    public GameObject canvasToHide;
    public GameObject bootsObject;
    public GameObject objectToShow;
    public GameObject objectToHide;
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

        if (canvasToHide != null)
            canvasToHide.SetActive(false);

        if (snailAudio != null)
        {
            snailAudio.Play();
            yield return new WaitForSeconds(snailAudio.clip.length);
        }

        if (bootsObject != null)
            bootsObject.SetActive(true);

        if (objectToShow != null)
            objectToShow.SetActive(true);

        if (objectToHide != null)
            objectToHide.SetActive(false);

        Debug.Log("Workbench abgeschlossen!");
    }
}
