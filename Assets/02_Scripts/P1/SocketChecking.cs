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
    public AudioSource successSound;

    public delegate void SuccessEvent();
    public event SuccessEvent OnSuccess;

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

        if (allCorrect && successSound != null)
        {
            Debug.Log("Alle Objekte korrekt. Spiele Sound ab.");
            successSound.Play();

            OnSuccess?.Invoke();
        }
        else
        {
            Debug.Log("SocketCheck: Noch nicht korrekt oder unvollständig.");
        }
    }
}
