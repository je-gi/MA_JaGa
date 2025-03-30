using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PlugController : MonoBehaviour
{
    public Transform startPosition;  // Die Startposition des Plugs in der Halterung
    public Transform endPosition;    // Die Zielposition, in die der Plug eingesteckt wird
    private XRGrabInteractable grabInteractable;
    private RopeController ropeController;  // Referenz zum RopeController-Skript

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        ropeController = FindObjectOfType<RopeController>();  // Finde das Seilcontroller-Skript
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Wenn der Plug gegriffen wird, starte die Interaktion
        ropeController.OnGrab();
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // Wenn der Plug losgelassen wird, prüfe, ob er eingesteckt ist
        if (Vector3.Distance(transform.position, endPosition.position) < 0.1f)
        {
            // Wenn der Plug nahe genug am Ziel ist, dann ist er eingesteckt
            ropeController.OnPlugInserted();
        }
        else
        {
            // Wenn er nicht in der Nähe des Zielpunkts ist, ziehe den Plug zurück
            ropeController.OnPlugRemoved();
        }
    }
}
