using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class MagneticSocketManager : MonoBehaviour
{
    [System.Serializable]
    public class SocketMapping
    {
        public XRSocketInteractor socket;
        public List<string> validTags; // Liste der erlaubten Tags für diesen Socket
    }

    public List<SocketMapping> socketMappings;

    // Prüft alle Sockets, ob die richtigen Cubes platziert sind
    public bool CheckCompletion()
    {
        foreach (SocketMapping mapping in socketMappings)
        {
            IXRSelectInteractable selectedObject = mapping.socket.GetOldestInteractableSelected();

            // Prüfen, ob ein Objekt im Socket ist
            if (selectedObject != null)
            {
                string objectTag = selectedObject.transform.tag;
                Debug.Log($"🔍 {selectedObject.transform.name} befindet sich in {mapping.socket.transform.name} mit Tag [{objectTag}]");

                // Überprüfung der gültigen Tags
                if (!mapping.validTags.Contains(objectTag))
                {
                    Debug.LogWarning($"❌ Falscher Cube im Socket {mapping.socket.transform.name}: {selectedObject.transform.name}");
                    return false;
                }
            }
            else
            {
                Debug.LogWarning($"⚠️ Socket {mapping.socket.transform.name} ist leer!");
                return false;
            }
        }

        Debug.Log("✅ Alle Sockets sind korrekt bestückt!");
        return true;
    }
}
