using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketChecking : MonoBehaviour
{
    [System.Serializable]
    public class SocketObjectPair
    {
        public XRSocketInteractor socket;  // Der Socket, in den das Objekt platziert werden soll
    }

    public SocketObjectPair[] socketObjectPairs;  // Liste von Sockets

    public GameObject[] setObjects; // Array f�r alle Sets
    public AudioSource successSound;  // Erfolgsger�usch

    private int currentSetIndex = 0;  // Der Index des aktuell angezeigten Sets

    void Start()
    {
        // Zu Beginn wird nur Set 1 angezeigt
        SpawnSet(currentSetIndex);
    }

    // Methode zum Spawnen eines Sets
    void SpawnSet(int setIndex)
    {
        // Alle Sets ausblenden
        foreach (var setObject in setObjects)
        {
            setObject.SetActive(false); // Alle Sets ausblenden
        }

        // Das Set an der angegebenen Indexposition einblenden
        if (setIndex < setObjects.Length)
        {
            setObjects[setIndex].SetActive(true); // Set wird aktiviert
        }
    }

    // Methode, die beim Klicken des 3D-Buttons aufgerufen wird
    public void NextSet()
    {
        // �berpr�fen, ob alle Sockets belegt sind
        if (AreAllSocketsFilled())
        {
            // Wenn alle Sockets belegt sind, spawne das n�chste Set
            currentSetIndex++;

            // Wenn das n�chste Set existiert, spawnen wir es
            if (currentSetIndex < setObjects.Length)
            {
                SpawnSet(currentSetIndex);
            }
            else
            {
                Debug.Log("Alle Sets abgeschlossen!");
            }
        }
        else
        {
            Debug.Log("Alle Sockets m�ssen belegt sein, bevor der Button funktioniert.");
        }
    }

    // Methode zum �berpr�fen, ob alle Sockets belegt sind
    bool AreAllSocketsFilled()
    {
        foreach (var pair in socketObjectPairs)
        {
            IXRSelectInteractable obj = pair.socket.GetOldestInteractableSelected();
            if (obj == null)
            {
                return false; // Wenn ein Socket leer ist, zur�ckkehren
            }
        }
        return true; // Alle Sockets sind belegt
    }

    // Diese Methode �berpr�ft, ob alle Sockets korrekt bef�llt sind (optional, f�r k�nftige Erweiterungen)
    public void socketCheck()
    {
        bool allSocketsFilled = true;

        foreach (var pair in socketObjectPairs)
        {
            IXRSelectInteractable objName = pair.socket.GetOldestInteractableSelected();

            if (objName == null)
            {
                allSocketsFilled = false;
                break;  // Wenn ein Socket leer ist, keine Notwendigkeit, weiter zu pr�fen
            }
        }

        // Wenn alle Sockets belegt sind, spiele den Erfolgston ab
        if (allSocketsFilled && successSound != null)
        {
            successSound.Play(); // Erfolgsger�usch
        }
    }
}
