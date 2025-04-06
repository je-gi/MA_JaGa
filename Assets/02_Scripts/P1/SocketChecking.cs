using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketChecking : MonoBehaviour
{
    [System.Serializable]
    public class SocketObjectPair
    {
        public XRSocketInteractor socket;
        public GameObject correctObject;
    }

    public SocketObjectPair[] socketObjectPairs;

    public delegate void PuzzleStatusEvent(bool isCompleted);
    public event PuzzleStatusEvent OnPuzzleStatusChanged;

    public void socketCheck()
    {
        bool allCorrect = true;

        foreach (var pair in socketObjectPairs)
        {
            IXRSelectInteractable objName = pair.socket.GetOldestInteractableSelected();

            if (objName != null)
            {
                if (objName.transform.gameObject != pair.correctObject)
                {
                    allCorrect = false;
                }
            }
            else
            {
                allCorrect = false;
            }
        }

        if (allCorrect)
        {
            Debug.Log("Alle Objekte korrekt.");
            OnPuzzleStatusChanged?.Invoke(true); 
        }
        else
        {
            Debug.Log("SocketCheck: Noch nicht korrekt oder unvollständig.");
            OnPuzzleStatusChanged?.Invoke(false);
        }
    }
}
