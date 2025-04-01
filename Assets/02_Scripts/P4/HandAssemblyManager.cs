using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HandAssemblyManager : MonoBehaviour
{
    public List<XRSocketInteractor> sockets;
    public List<GameObject> objectsToHide;
    public List<GameObject> objectsToShow;

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
        for (int i = 0; i < sockets.Count; i++)
        {
            sockets[i].socketActive = (i == 0);
        }
    }

    private void CheckSocketCompletion()
    {
        if (currentSocketIndex >= sockets.Count) return;

        if (sockets[currentSocketIndex].hasSelection)
        {
            currentSocketIndex++;
            if (currentSocketIndex < sockets.Count)
            {
                sockets[currentSocketIndex].socketActive = true;
            }
            else
            {
                isMinigameCompleted = true;
                OnMinigameCompleted();
            }
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
