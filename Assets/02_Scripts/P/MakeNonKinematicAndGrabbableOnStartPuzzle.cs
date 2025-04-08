using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class MakeNonKinematicAndGrabbableOnStartPuzzle : MonoBehaviour
{
    public List<GameObject> objectsToManage;
    public MonoBehaviour puzzleManager;

    private bool hasActivated = false;

    void Start()
    {
        SetObjectsInitialState(false);
    }

    void Update()
    {
        if (!hasActivated && IsPuzzleStarted())
        {
            SetObjectsInitialState(true);
            hasActivated = true;
        }
    }

    private void SetObjectsInitialState(bool enable)
    {
        foreach (var obj in objectsToManage)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            XRGrabInteractable grab = obj.GetComponent<XRGrabInteractable>();

            if (rb != null)
                rb.isKinematic = !enable;

            if (grab != null)
                grab.enabled = enable;
        }
    }

    private bool IsPuzzleStarted()
    {
        if (puzzleManager == null) return false;

        var hasStartedField = puzzleManager.GetType().GetField("hasStarted", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (hasStartedField != null)
        {
            return (bool)hasStartedField.GetValue(puzzleManager);
        }

        return false;
    }
}
