using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GlassesShow : MonoBehaviour
{
    public XRSocketInteractor glassesSocket;

    [Header("manually assigned eye objects")]
    public List<GameObject> manuallyAssignedEyeObjects;

    private List<GameObject> eyeObjectsInScene = new List<GameObject>();

    private void Start()
    {
        foreach (var obj in manuallyAssignedEyeObjects)
        {
            RegisterEyeObject(obj);
        }

        SetAllEyeObjectsActive(false); 
    }

    private void Update()
    {
        if (glassesSocket.hasSelection)
        {
            ShowAllEyeObjects();
        }
        else
        {
            SetAllEyeObjectsActive(false);
        }
    }

    public void RegisterEyeObject(GameObject newEye)
    {
        if (newEye != null && !eyeObjectsInScene.Contains(newEye))
        {
            eyeObjectsInScene.Add(newEye);
        }
    }

    public void UnregisterEyeObject(GameObject eye)
    {
        if (eyeObjectsInScene.Contains(eye))
        {
            eyeObjectsInScene.Remove(eye);
        }
    }

    private void ShowAllEyeObjects()
    {
        foreach (var eye in eyeObjectsInScene)
        {
            if (eye != null)
            {
                eye.SetActive(true);
            }
        }
    }

    private void SetAllEyeObjectsActive(bool isActive)
    {
        foreach (var eye in eyeObjectsInScene)
        {
            if (eye != null)
            {
                eye.SetActive(isActive);
            }
        }
    }
}
