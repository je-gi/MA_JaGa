using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class MiniGameSocketChecker : MonoBehaviour
{
    [System.Serializable]
    public class SocketCheck
    {
        public XRSocketInteractor socket;
        public GameObject[] validObjects;
    }

    public SocketCheck[] socketChecks;

    public bool AreAllSocketsCorrect()
    {
        foreach (var check in socketChecks)
        {
            if (!check.socket.hasSelection)
                return false;

            var selectedObject = check.socket.GetOldestInteractableSelected().transform.gameObject;

            bool isValid = false;
            foreach (var validObject in check.validObjects)
            {
                if (selectedObject == validObject)
                {
                    isValid = true;
                    break;
                }
            }

            if (!isValid)
                return false;
        }

        return true; 
    }
}
