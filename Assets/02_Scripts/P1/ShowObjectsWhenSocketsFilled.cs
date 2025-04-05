using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ShowObjectsWhenSocketsFilled : MonoBehaviour
{
    public XRSocketInteractor socket1;
    public XRSocketInteractor socket2;

    public GameObject objectToShow1;
    public GameObject objectToShow2;

    private bool alreadyShown = false;

    void Update()
    {
        if (!alreadyShown && BothSocketsFilled())
        {
            objectToShow1.SetActive(true);
            objectToShow2.SetActive(true);
            alreadyShown = true;
        }
    }

    bool BothSocketsFilled()
    {
        return socket1.GetOldestInteractableSelected() != null && socket2.GetOldestInteractableSelected() != null;
    }
}
