using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine;

public class ShowObjectsWhenSocketsFilled : MonoBehaviour
{
    public XRSocketInteractor triggerSocket;

    public GameObject objectToShow1;
    public GameObject objectToShow2;

    private bool alreadyShown = false;
    public bool AlreadyShown => alreadyShown;

    void Update()
    {
        if (!alreadyShown && triggerSocket.GetOldestInteractableSelected() != null)
        {
            ShowObjects();
        }
    }

    public void ShowObjects()
    {
        if (alreadyShown) return;

        objectToShow1.SetActive(true);
        objectToShow2.SetActive(true);
        alreadyShown = true;
    }
}
