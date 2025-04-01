using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HandAssemblyManager : MonoBehaviour
{
    public List<XRSocketInteractor> sockets;
    public List<GameObject> objectsToHide;
    public List<GameObject> objectsToShow;
    public Spraycan sprayScript;
    public Shrinkgun shrinkScript;

    private int currentSocketIndex = 0;
    private bool isMinigameCompleted = false;

    private void Start()
    {
        InitializeSockets();
    }

    private void Update()
    {
        if (!isMinigameCompleted)
        {
            CheckSocketCompletion();
        }
    }

    private void InitializeSockets()
    {
        for (int i = 1; i < sockets.Count; i++)
        {
            sockets[i].socketActive = false;
        }
    }

    private void CheckSocketCompletion()
    {
        if (currentSocketIndex >= sockets.Count) return;

        if (currentSocketIndex == 0 && sockets[0].hasSelection)
        {
            currentSocketIndex++;
            sockets[1].socketActive = true;
        }

        if (currentSocketIndex == 1 && sockets[1].hasSelection)
        {
            currentSocketIndex++;
            sockets[2].socketActive = true;
        }

        if (currentSocketIndex == 2 && sockets[2].hasSelection && sprayScript.IsSpraySuccessful())
        {
            currentSocketIndex++;
            sockets[3].socketActive = true;
        }

        if (currentSocketIndex == 3 && sockets[3].hasSelection && shrinkScript.IsShrinkSuccessful())
        {
            isMinigameCompleted = true;
            OnMinigameCompleted();
        }
    }

    private void OnMinigameCompleted()
    {
        HideObjects();
        ShowObjects();
    }

    private void HideObjects()
    {
        foreach (var obj in objectsToHide)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    private void ShowObjects()
    {
        foreach (var obj in objectsToShow)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }
}
