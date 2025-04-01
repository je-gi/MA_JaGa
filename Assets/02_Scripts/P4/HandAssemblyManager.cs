using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HandAssemblyManager : MonoBehaviour
{
    public List<XRSocketInteractor> sockets;      // Liste der Sockets
    public List<GameObject> objectsToHide;       // Objekte, die ausgeblendet werden sollen
    public List<GameObject> objectsToShow;       // Objekte, die eingeblendet werden sollen

    private int currentSocketIndex = 0;
    private bool isMinigameCompleted = false;    // Wird auf true gesetzt, wenn das Minigame abgeschlossen ist

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
        // Alle Sockets deaktivieren außer den ersten
        for (int i = 0; i < sockets.Count; i++)
        {
            sockets[i].socketActive = (i == 0); // Nur der erste Socket ist aktiv
        }
    }

    private void CheckSocketCompletion()
    {
        // Sicherstellen, dass wir nicht über die Anzahl der Sockets hinausgehen
        if (currentSocketIndex >= sockets.Count) return;

        // Prüfen, ob der aktuelle Socket belegt ist
        if (sockets[currentSocketIndex].hasSelection)
        {
            Debug.Log($"✔ Objekt in Socket {currentSocketIndex + 1} platziert");

            // Nächsten Socket aktivieren, sobald der aktuelle belegt ist
            currentSocketIndex++;
            if (currentSocketIndex < sockets.Count)
            {
                sockets[currentSocketIndex].socketActive = true;  // Nächsten Socket aktivieren
            }
            else
            {
                Debug.Log("🎉 Alle Objekte platziert - Minigame abgeschlossen!");
                isMinigameCompleted = true;  // Markiere das Minigame als abgeschlossen
                OnMinigameCompleted();       // Minigame abgeschlossen, führe Aktionen aus
            }
        }
    }

    private void OnMinigameCompleted()
    {
        // Erst jetzt die Objekte ein- und ausblenden, nachdem das Minigame abgeschlossen ist
        HideObjects();  // Ausblenden der Objekte
        ShowObjects();  // Einblenden der Objekte
    }

    private void HideObjects()
    {
        // Alle Objekte ausblenden, die in der Liste sind
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
        // Alle Objekte einblenden, die in der Liste sind
        foreach (var obj in objectsToShow)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }
}
