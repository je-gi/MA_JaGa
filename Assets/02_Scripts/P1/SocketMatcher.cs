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
        public XRSocketInteractor socket;
        public GameObject correctObject;
    }

    public SocketObjectPair[] socketObjectPairs;
    public AudioSource successSound;

    void Start()
    {
    }

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
            successSound.Play();
        }
    }
}
